using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

//ステージがスライドしているときスコア加算
public class Score : MonoBehaviour
{

    private StageController stageController;
    private Text textMesh;

    public float scoreRate = 1.0f;
    private void Start()
    {
        stageController = GameObject.FindObjectOfType<StageController>();
        textMesh = GetComponent<Text>();

        this.UpdateAsObservable().Where(_ => stageController.isMoving).Subscribe(_ => { ScoreIncrement(); });
    }

    private void Update()
    {
        textMesh.text = "<color=#ccccff>Score</color> " + ((int)GameStateManager.Instance.Score).ToString();
    }
    private void ScoreIncrement()
    {
        GameStateManager.Instance.Score += (stageController.GetStageSpeed() / stageController.lowSpeed) * scoreRate;
    }

    public void CallScoreUp(int score,int initScore,float duration)
    {
        StartCoroutine(ScoreUp(score,initScore, duration));
    }
    //スコアアップアイテムを取った時の処理
    private IEnumerator ScoreUp(int score,int initScore,float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime && GameStateManager.Instance.Score < initScore + score)
        {
            GameStateManager.Instance.Score += (score / duration) * Time.deltaTime;

            //１フレーム待つ
            yield return null;
        }
        yield break;
    }
    public float GetScore()
    {
        return GameStateManager.Instance.Score;
    }
}
