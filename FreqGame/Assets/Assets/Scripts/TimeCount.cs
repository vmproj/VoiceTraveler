using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

public class TimeCount : SingletonMonoBehaviour<TimeCount>
{

    //測定を始めた時間
    float startTime;
    //残り時間
    private float initRemaingTime = 180f;
    public float remainingTime;
    public bool isTimer = false;

    public bool isTimeOver = false;

    private Coroutine coroutine;
    private playerController playerController;
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //Juliusがスタンバイ状態になったらカウント
        this.ObserveEveryValueChanged(x => isTimer).Where(x => x).
            Subscribe(_ =>
            {
                startTime = Time.time;
                if (SceneManager.GetActiveScene().name.Contains("Game"))
                {
                    //違うシーンから来た時
                    if (!GameStateManager.Instance.isSameState())
                        SetInitTime();

                    if (GameStateManager.Instance.isReload)
                    {
                        GameStateManager.Instance.isReload = false;
                        SetInitTime();
                    }
                    coroutine = StartCoroutine(TimeCountStart());
                }
            });
        this.ObserveEveryValueChanged(x => isTimer).Where(x => !x).
            Subscribe(_ =>
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
            });
    }
    public void isTimerStart()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(1)).Subscribe(_ => isTimer = true);
        if (remainingTime <= 0f)
            SetInitTime();
    }

    private IEnumerator TimeCountStart()
    {
        while (true)
        {
            //小数点を省いた時間差分
            remainingTime -= Time.time - startTime;
            startTime = Time.time;

            if (remainingTime <= 0f)
            {
                remainingTime = 0.00f;
                playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
                StartCoroutine(playerController.Die());
                //スタート位置へ
                RespawnManager.Instance.isRespawn = false;
                isTimeOver = true;
                break;
            }
            yield return null;
        }
    }

    public void SetInitTime()
    {
        remainingTime = initRemaingTime;
        isTimeOver = false;
    }
}
