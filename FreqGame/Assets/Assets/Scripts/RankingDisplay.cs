using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//難易度を取得し、読み込むファイルを指定。何位かどうか表示する。
public class RankingDisplay : RankingBase
{
    [SerializeField]private string datapath = "Score/Easy";
    private int preScore = 10000;
    private int remainingTime = 1500;
    private int totalScore = 0;

    private int subPerFrameScore = 50;
    private int subPerFrameTime = 10;

    private Text preScoreText;
    private Text timeText;
    private Text totalScoreText;
    private Text rankText;

    //UIを消すオブジェクトリスト
    private Image scoreButtonImage;
    private Image timeButtonImage;
    private Text timeT;
    private Text scoreT;


    //ランキング表示
    private Image inputField;
    private Text placeHolder;

    //トータルスコアを移動させる要素
    private RectTransform totalScoreButton;
    private Vector3 lastTotalScorePostion = new Vector3(0f, 80f, 0f);

    private AudioSource aud;
    [SerializeField]
    private AudioClip[] se;

    private void Start()
    {
        StartCoroutine(SceneChange.FadeOut());
        if (GameStateManager.Instance.Score != 0f)
            preScore = (int)GameStateManager.Instance.Score;
        remainingTime = (int)TimeCount.Instance.remainingTime;
        //preScoreとのバランスをとるために10倍する
        remainingTime *= 10;

        //Normal
        if (GameStateManager.Instance.prestate == GameStateManager.State.Game2)
            datapath = "Score/Normal";
        else if (GameStateManager.Instance.prestate == GameStateManager.State.Game3)
            datapath = "Score/Hard";
        else if (GameStateManager.Instance.prestate == GameStateManager.State.Game4)
            datapath = "Score/EX";


        preScoreText = GameObject.Find("Canvas/ScoreButton/Text").GetComponent<Text>();
        timeText = GameObject.Find("Canvas/TimeButton/Text").GetComponent<Text>();
        totalScoreText = GameObject.Find("Canvas/TotalScoreButton/Text").GetComponent<Text>();
        rankText = GameObject.Find("Canvas/RankDisplay/Text").GetComponent<Text>();
        inputField = GameObject.Find("Canvas/InputField").GetComponent<Image>();
        placeHolder = GameObject.Find("Canvas/InputField/Placeholder").GetComponent<Text>();

        scoreButtonImage = GameObject.Find("Canvas/ScoreButton").GetComponent<Image>();
        timeButtonImage = GameObject.Find("Canvas/TimeButton").GetComponent<Image>();
        timeT = GameObject.Find("Canvas/TimeButton/TimeText").GetComponent<Text>();
        scoreT = GameObject.Find("Canvas/ScoreButton/ScoreText").GetComponent<Text>();


        totalScoreButton = GameObject.Find("Canvas/TotalScoreButton").GetComponent<RectTransform>();

        subPerFrameScore = preScore / 100;
        subPerFrameTime = remainingTime / 100;

        preScoreText.text = preScore.ToString();
        timeText.text = remainingTime.ToString();
        totalScoreText.text = totalScore.ToString();


        aud = GetComponent<AudioSource>();

        ReadFile(datapath);
        StartCoroutine(RankAnimation());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //名前入力ボタンが登場したら
            if (placeHolder.color.a >= 0.5f)
            {
                SaveRanking(totalScore, inputField.GetComponent<InputField>().text, datapath);
                Destroy(inputField.gameObject);
                StartCoroutine(SceneChange.ChangeScene("Title"));
            }
        }
    }
    bool countingIsOver = false;
    IEnumerator Sound()
    {
        float t = 0;
        while (!countingIsOver)
        {
            t += Time.deltaTime;
            if (0.06f < t)
            {
                aud.Play();
                t = 0;
            }
            yield return null;
        }
        aud.clip = se[1];
        aud.Play();
        yield break;
    }
    IEnumerator RankAnimation()
    {
        aud.volume = 0.5f;
        aud.clip = se[0];
        yield return new WaitForSeconds(1);
        StartCoroutine(Sound());
        while (preScore > subPerFrameScore - 1)
        {
            preScore -= subPerFrameScore;
            totalScore += subPerFrameScore;
            preScoreText.text = preScore.ToString();
            totalScoreText.text = totalScore.ToString();
            yield return new WaitForSeconds(0.01f);
        }
        totalScore += preScore;
        preScore = 0;
        preScoreText.text = preScore.ToString();
        totalScoreText.text = totalScore.ToString();

        while (remainingTime > subPerFrameTime - 1)
        {
            if (remainingTime <= 0)
                break;

            remainingTime -= subPerFrameTime;
            totalScore += subPerFrameTime;

            timeText.text = remainingTime.ToString();
            totalScoreText.text = totalScore.ToString();

            yield return new WaitForSeconds(0.01f);
        }
        countingIsOver = true;
        totalScore += remainingTime;
        remainingTime = 0;
        timeText.text = remainingTime.ToString();
        totalScoreText.text = totalScore.ToString();
        while (totalScoreButton.localPosition.y > lastTotalScorePostion.y)
        {
            //TotalScoreのUIを移動させる
            totalScoreButton.localPosition = Vector3.Lerp(totalScoreButton.localPosition, lastTotalScorePostion, Time.deltaTime * 5);
            totalScoreButton.localScale *= 1.007f;
            if (totalScoreButton.localScale.x > 1.4f)
                break;
            yield return null;
        }
        while (scoreButtonImage.color.a > 0)
        {
            scoreButtonImage.color -= Color.black * 0.05f;
            timeButtonImage.color -= Color.black * 0.05f;
            timeT.color -= Color.black * 0.05f;
            scoreT.color -= Color.black * 0.05f;
            preScoreText.color -= Color.black * 0.05f;
            timeText.color -= Color.black * 0.05f;
            yield return null;
        }

        var rank = Return_Rankin(datapath, totalScore);
        if (rank == -1)
        {
            rankText.text = "ランクインならず．．．";
        }
        else
            rankText.text = string.Format("第{0}位", rank).ToString();
        while (rankText.color.a < 1)
        {
            rankText.color += Color.black * 0.05f;
            yield return null;
        }
        if (rank == -1)
        {
            StartCoroutine(SceneChange.ChangeScene("Title"));
            yield break;
        }

        while (inputField.color.a < 1)
        {
            inputField.color += Color.black * 0.1f;
            yield return null;
        }

        while (placeHolder.color.a < 0.5f)
        {
            placeHolder.color += Color.black * 0.1f;
            yield return null;
        }
        yield break;
    }
}
