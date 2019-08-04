using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    private Score score;
    [SerializeField, Tooltip("加算する得点")] int point;
    [SerializeField, Tooltip("加算する時間")] float duration;

    [SerializeField]private FadeText fadeText;
    [SerializeField]private UpGameObject upGameObject;
    StageController stageController;
    [SerializeField]
    AudioClip se;
    Collider2D col;
    private void Start()
    {
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
        col = GetComponent<Collider2D>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        //fadeText = GameObject.Find("ScoreText").GetComponent<FadeText>();
        //upGameObject = GameObject.Find("ScoreText").GetComponent<UpGameObject>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            stageController.PlaySE(se);
            score.CallScoreUp(point, (int)score.GetScore(),duration);
            fadeText.ScoreDisplay(point.ToString());
            upGameObject.UpStart();
            StartCoroutine(Vanish());
        }
    }
    private IEnumerator Vanish()
    {
        col.enabled = false;
        float t = 0;
        Vector3 def = transform.localScale;
        GetComponentInChildren<ParticleSystem>().Stop();
        StartCoroutine("HeartEffect");
        while (t <= 1)
        {
            t += Time.deltaTime * 2f;
            transform.localScale = Vector3.Lerp(def, Vector3.zero, t);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        yield break;
    }
    private IEnumerator HeartEffect()
    {
        float t = 0;
        Transform heart = transform.parent.GetChild(2);
        SpriteRenderer heartSpr = heart.GetComponent<SpriteRenderer>();
        while (t <= 1)
        {
            t += Time.deltaTime;
            heart.localScale = Vector3.Lerp(Vector3.one * 2f, Vector3.one * 5, t);
            heartSpr.color = Color.Lerp(Color.white, Color.white - Color.black, t);
            yield return null;
        }
        Destroy(heart.gameObject);
        yield break;
    }
}
