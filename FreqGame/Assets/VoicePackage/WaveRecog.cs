using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Frequency
{
    public class WaveRecog : MonoBehaviour
    {

        AudioSource audioSource;
        private float[] sampleData = new float[1024];

        //周波数を表示する
        [SerializeField]private Text freqText;

        //int preFreq = int.MaxValue;
        int freq = int.MaxValue;
        double dFreq = 0;
        public float DFreq { get { return (float)dFreq; } }

        //周波数の最大値・最小値,テキスト
        public int maxFreq = int.MinValue, minFreq = int.MaxValue;
        [SerializeField]private Text maxFText, minFText;

        //周波数検知回数,<周波数,回数>
        private SortedDictionary<int, int> freqCount = new SortedDictionary<int, int>();

        //最大値・最小値として登録する周波数回数
        private int registerCount = 5;

        //必要なパワー平均値(dB)
        int powerAverageValue = -20;

        public void SetActivate()
        {
            this.gameObject.SetActive(true);
        }

        //ゲームオブジェクトがアクティヴになったら
        void OnEnable()
        {

            //マイク存在確認
            if (Microphone.devices.Length == 0)
            {
                Debug.Log("マイクが見つかりません");
                Destroy(GetComponent<WaveRecog>());
                return;
            }

            audioSource = this.GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = Microphone.Start(null, true, 10, 44100);
            //マイクが待機状態になるまで待つ
            while (!(Microphone.GetPosition(null) > 0)) { }
            audioSource.Play();

        }

        void Update()
        {
            audioSource.GetOutputData(sampleData, 1);
            double? dfreq = PitchAccord.EstimateBasicFrequency(AudioSettings.outputSampleRate, sampleData, powerAverageValue);
            dFreq = dfreq ?? 0;
            if (dfreq.HasValue)
            {
                freq = (int)dfreq;
                freqText.text = freq.ToString() + "Hz";
                if (freqCount.ContainsKey(freq))
                {
                    freqCount[freq]++;
                    if (freqCount[freq] >= registerCount)
                    {
                        MaxFreqCheck(freq);
                        MinFreqCheck(freq);
                    }
                }
                else
                    freqCount[freq] = 1;
            }
            else
                freqText.text = "0 Hz";

        }

        //最大の周波数チェック
        private void MaxFreqCheck(int freq)
        {
            if (freq > maxFreq)
            {
                maxFreq = freq;
                maxFText.text = String.Format("max  {0} Hz", maxFreq);
            }
        }
        private void MinFreqCheck(int freq)
        {
            if (freq < minFreq)
            {
                minFreq = freq;
                minFText.text = String.Format("min  {0} Hz", minFreq);
            }
        }

    }
}