using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GateObserver : CollisionGimmick
{

    bool playerHasCome;
    [SerializeField]
    string password;
    //マイクがないときゲートを開く(デバック用)
    [SerializeField] bool opened;
    public bool Opened
    {
        get { return opened; }
        set { opened = value; }
    }
    Text passwordText;

    float time = 0;//test

    Recognition recog;
    WordDisplay wordDisplay;

    VoiceSwitchManager voiceSwitchManager;

    AudioSource audioSource;
    void Start()
    {
        passwordText = GameObject.Find("Password").GetComponent<Text>();
        recog = GameObject.Find("Recognition").GetComponent<Recognition>();
        wordDisplay = GameObject.Find("Canvas/VoiceWord").GetComponent<WordDisplay>();

        voiceSwitchManager = GameObject.Find("VoiceSwitchManager").GetComponent<VoiceSwitchManager>();

        this.ObserveEveryValueChanged(x => opened).Where(x => x).
            Subscribe(_ => SwitchVoice());

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            recog.VoiceApply(password);
            wordDisplay.Display();
            voiceSwitchManager.SwitchVoice(true);
            StartCoroutine("AnimatePassword");
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            passwordText.text = password;
            playerHasCome = true;
            Opened = recog.isOpen;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerHasCome = false;
            wordDisplay.Display();
            passwordText.text = "";
            StopCoroutine("AnimatePassword");
        }
    }
    void SwitchVoice()
    {
        if (playerHasCome)
        {
            voiceSwitchManager.SwitchVoice(false);
            audioSource.Play();
        }
    }
    IEnumerator AnimatePassword()
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime;

            yield return null;
        }
    }

}
