using Frequency;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ゲーム中で使う周波数取得クラス
public class FreqRecogInGame : MonoBehaviour
{

    AudioSource audioSource;
    private float[] sampleData = new float[1024];
    int max = 300, min = 80;

    //必要なパワー平均値(dB)
    int powerAverageValue = -20;

    //最大パワー(dB)
    int maxPowerValue = 5;

    int? dB;
    //正規化
    float normalizedValue;
    float maxspeed, minspeed;
    private playerController pC;
    private StageController sC;

    Text freqText;
    JsonProcess jsonProcess;

    void Awake()
    {
        //マイク存在確認
        if (Microphone.devices.Length == 0)
        {
            Debug.Log("マイクが見つかりません");
            Destroy(GetComponent<FreqRecogInGame>());
            return;
        }

        audioSource = this.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        //マイクが待機状態になるまで待つ
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();

        pC = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
        freqText = GameObject.Find("Canvas/Frequency").GetComponent<Text>();
        sC = GameObject.FindObjectOfType<StageController>();
        maxspeed = sC.highSpeed;
        minspeed = sC.lowSpeed;

        jsonProcess = GameObject.Find("VoiceData").GetComponent<JsonProcess>();
        int tmp;
        if (int.TryParse(jsonProcess.voiceData.maxFreq, out tmp))
            max = int.Parse(jsonProcess.voiceData.maxFreq);
        if (int.TryParse(jsonProcess.voiceData.minFreq, out tmp))
            min = int.Parse(jsonProcess.voiceData.minFreq);
    }

    private void Update()
    {
        audioSource.GetOutputData(sampleData, 1);
        double? dfreq = PitchAccord.EstimateBasicFrequency(AudioSettings.outputSampleRate, sampleData, powerAverageValue);
        if (dfreq.HasValue)
        {
            int freq = (int)dfreq;

            freqText.text = string.Format("Freq {0}Hz\nMax {1}Hz\nMin {2}Hz", freq, max, min);
            freq = Mathf.Clamp(freq, min, max);
            pC.Movement = (float)(freq - min) / (max - min);

            //パワースペクトルからデシベルを呼び出す
            dB = PitchAccord.GetDB();
            if (dB.HasValue)
            {
                normalizedValue = (float)(dB.Value - powerAverageValue) / (maxPowerValue - powerAverageValue);
                var speed = (Mathf.Clamp(normalizedValue, 0f, 1f)) * (maxspeed - minspeed) + minspeed;
                sC.SetFreqSpeed(Mathf.Lerp(sC.GetFreqSpeed(),
                speed, Time.deltaTime * 1.5f));
            }
        }
        else
        {

            sC.SetFreqSpeed(Mathf.Lerp(sC.GetFreqSpeed(),
            minspeed, Time.deltaTime * 1.5f));

            freqText.text = string.Format("Freq 0Hz\nMax {0}Hz\nMin {1}Hz", max, min);
        }
    }
}
