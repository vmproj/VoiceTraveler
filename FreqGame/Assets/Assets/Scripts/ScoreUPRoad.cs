using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class ScoreUPRoad : MonoBehaviour
{
    StageController stageController;

    [SerializeField]
    AudioClip se;
    private Score score;
    [SerializeField] private GameObject scoreTextPrefab;
    private ScoreRateFadeText scoreRateFadeText;
    Transform canvas;
    Transform player;
    bool isTextProduce = false;

    SpriteRenderer spr;
    Color defColor;
    Color emissiveColor;
    float t = 0;
    private void Start()
    {
        stageController = GameObject.Find("StageController").GetComponent<StageController>();

        score = GameObject.FindObjectOfType<Score>();
        scoreRateFadeText = GameObject.FindObjectOfType<ScoreRateFadeText>();
        canvas = GameObject.Find("Canvas").GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        spr = GetComponent<SpriteRenderer>();
        defColor = spr.color;
        emissiveColor = defColor;
        emissiveColor.a = 0.6f;
        Observable.Interval(TimeSpan.FromMilliseconds(300)).Where(x => isTextProduce == true).Subscribe(_ => ProduceText());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            stageController.ShotSE(se);
            score.scoreRate = 5.0f;
            scoreRateFadeText.text.text = string.Format("×{0}.0", score.scoreRate.ToString()); ;
            //scoreRateFadeText.ScoreDisplay(score.scoreRate.ToString());
            isTextProduce = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            t += Time.deltaTime * 2f;
            spr.color = Color.Lerp(emissiveColor, defColor, t);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            score.scoreRate = 1.0f;
            scoreRateFadeText.ScoreDisplay(score.scoreRate.ToString());
            isTextProduce = false;
            spr.color = defColor;
        }
    }
    private void ProduceText()
    {
        //キャンバスに移動する際、y軸に294下がる,Playerの範囲-5～5,キャンバス範囲-300～300,
        //プレイヤーの上に配置するのでオフセット50くらい
        Instantiate(scoreTextPrefab,
            new Vector3(scoreTextPrefab.transform.position.x+80, player.transform.position.y * 60 + 294 + 50, scoreTextPrefab.transform.position.z),
            Quaternion.identity, canvas);
    }
}
