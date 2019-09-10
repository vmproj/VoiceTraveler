using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//音声認識と周波数取得を切り替えるクラス
public class VoiceSwitchManager : MonoBehaviour
{

    [SerializeField] private GameObject FreqRecogInGame;
    [SerializeField] private GameObject julius;
    private WordDisplay wordDisplay;

    private GameObject cloneFreqRecogInGame;
    private GameObject cloneJulius;
    private void Start()
    {
        wordDisplay = GameObject.Find("Canvas/VoiceWord").GetComponent<WordDisplay>();
        cloneFreqRecogInGame = Instantiate(FreqRecogInGame);
    }

    public void SwitchVoice(bool isVoice)
    {
        //音声認識
        if (isVoice)
        {
            if (cloneFreqRecogInGame != null)
            {
                Destroy(cloneFreqRecogInGame);
            }
            //マイク存在確認
            if (Microphone.devices.Length > 0)
            {
                var gameObject = GameObject.Find("SynchronizationContextRunner");
                //これをとりのぞかないと、音声認識しない
                if (gameObject != null)
                    Destroy(gameObject);

                cloneJulius = Instantiate(julius);
                wordDisplay.FetchJulius();
            }
        }
        else if (!isVoice)
        {
            if (cloneJulius != null)
                Destroy(cloneJulius);
            cloneFreqRecogInGame = Instantiate(FreqRecogInGame);
        }
    }
}
