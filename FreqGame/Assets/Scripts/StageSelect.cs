using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    GameObject[] stagePointsObj;
    StagePoint[] stagePoint;
    [SerializeField]
    GameObject stagePoints;
    [SerializeField]
    Transform ufoT;
    [SerializeField]
    Transform cameraT;
    [SerializeField]
    Transform earthT;
    [SerializeField]
    LineRenderer stageLine;

    [SerializeField]
    GameObject explainObj;
    Text explainText;
    [SerializeField]
    Transform message;

    int select = 0;
    int prevSelect = 0;
    int stageCount;
    bool inputtable = false;
    bool isAfterSelection = false;

    Vector3 stageToCam = new Vector3(-1, 7, -8);
    Vector3 stageToUfo = new Vector3(0, 3, 0);
    Vector3[] cameraPoint;

    Color defEmissionColor;
    Color selectedEmissionColor = new Color(0.9f, 0.9f, 0.4f);

    AudioSource audioSource;
    [SerializeField]
    AudioClip[] se;

    void Start()
    {
        StartCoroutine(SceneChange.FadeOut(2));
        audioSource = GetComponent<AudioSource>();
        stageCount = stagePoints.transform.childCount;
        stagePointsObj = new GameObject[stageCount];
        stagePoint = new StagePoint[stageCount];
        for (int i = 0; i < stageCount; i++)
        {
            stagePointsObj[i] = stagePoints.transform.GetChild(i).gameObject;
            stagePoint[i] = stagePointsObj[i].GetComponent<StagePoint>();
        }
        cameraPoint = new Vector3[stageCount];
        cameraT.position = cameraPoint[0] + stageToCam;
        ufoT.position = stagePointsObj[0].transform.position + stageToUfo;
        defEmissionColor = stagePointsObj[0].GetComponent<Renderer>().materials[1].GetColor("_EmissionColor");
        stagePointsObj[select].GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", selectedEmissionColor);
        StartCoroutine(SetUp());
        for (int i = 0; i < stageCount; i++)
        {
            cameraPoint[i] = stagePointsObj[i].transform.position;
            cameraPoint[i].x /= 1.1f;
        }
        StartCoroutine(MoveCamera());

        explainText = explainObj.GetComponentInChildren<Text>();
        StartCoroutine("Explain");
    }

    IEnumerator SetUp()
    {
        yield return new WaitForSeconds(0.2f);
        float time = 0;
        stageLine.positionCount = 1;
        stageLine.SetPosition(0, stagePointsObj[0].transform.position);
        for (int i = 0; i < stageCount - 1; i++)
        {
            stageLine.positionCount = i + 2;
            time = 0;
            while (time <= 1)
            {
                time += Time.deltaTime * 3;
                stageLine.SetPosition(i + 1, Vector3.Lerp(stagePointsObj[i].transform.position, stagePointsObj[i + 1].transform.position, time));
                yield return null;
            }
        }
        inputtable = true;

    }
    IEnumerator PutOnStage(bool on)
    {
        float time = 0;
        while (time <= 1)
        {
            time += Time.deltaTime * (on ? 7 : 2);
            if (on) stagePointsObj[select].GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", Color.Lerp(defEmissionColor, selectedEmissionColor, time));
            else stagePointsObj[prevSelect].GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", Color.Lerp(selectedEmissionColor, defEmissionColor, time));
            yield return null;
        }
        yield break;
    }

    void Update()
    {

        if (Input.GetButtonDown("Horizontal") && inputtable)
        {
            audioSource.PlayOneShot(se[0]);
            prevSelect = select;
            select += (int)Input.GetAxisRaw("Horizontal");
            StopCoroutine("MoveUfo");
            if (select < 0) select = 0;
            else if (stageCount <= select) select = stageCount - 1;
            else
            {
                StartCoroutine("MoveUfo");
                StopCoroutine("Explain");
                StartCoroutine("Explain");

            }
        }
        if (isAfterSelection)
        {
            ufoT.position += Vector3.up * 0.2f;
        }
        else
        {
            ufoT.transform.LookAt(cameraT.transform.position);
            ufoT.GetChild(0).position += Vector3.up * Mathf.Sin(Time.time * 350 * Mathf.Deg2Rad) * 0.07f;
        }
        explainObj.transform.position += (Vector3.up + Vector3.right * 0.5f) * Mathf.Sin(Time.time * 420 * Mathf.Deg2Rad) * 0.02f;
        message.transform.position += (Vector3.up) * Mathf.Sin(Time.time * 330 * Mathf.Deg2Rad) * 0.01f;

        if (Input.GetKeyDown(KeyCode.Z) && inputtable&& stagePoint[select].CanGo)
        {
            audioSource.PlayOneShot(se[2]);
            isAfterSelection = true;
            Debug.Log(stagePoint[select].StageName);
            StartCoroutine(SceneChange.ChangeScene(stagePoint[select].StageName));
            //else SE
        }
    }


    IEnumerator Explain()
    {
        explainText.text = string.Empty;
        yield return new WaitForSeconds(0.3f);
        float time = 0;
        float speed = 3;
        while (time <= 1)
        {
            time += Time.deltaTime * 2.5f;
            explainText.text = LoomText("ステージ：", time * speed, "bbffbb") + LoomText((select + 1).ToString(), time * speed, "9999ff")
             + LoomText("\n" + "むずかしさ：", time * speed - speed / 4f * 1, "ffbbbb") + LoomText(LineUpStars(stagePoint[select].Difficulty).ToString(), time * speed - speed / 4f * 1, "ff6666")
              + LoomText("\n" + "ハイスコア：", time * speed - speed / 4f * 2, "ffffbb") + LoomText(stagePoint[select].HighScore.ToString(), time * speed - speed / 4f * 2, "66ff66")
            + LoomText("\n" + "なまえ：", time * speed - speed / 4f * 3, "bbbbff") + LoomText(stagePoint[select].UserName.ToString(), time * speed - speed / 4f * 3, "ffffff");
            yield return null;
        }
    }

    string LoomText(string text, float time, string color_xxxxxx)
    {
        int alpha = (int)(time * 240);
        if (alpha > 254) alpha = 254;
        if (alpha < 0) alpha = 0;
        return "<color=#" + color_xxxxxx + alpha.ToString("x2") + ">" + text + "</color>";
    }

    string LineUpStars(int count)
    {
        string stars = string.Empty;
        for (int i = 0; i < count; i++)
        {
            stars += "★";
        }
        return stars;
    }

    IEnumerator MoveCamera()
    {
        Vector3 velocity = Vector3.zero;
        while (true)
        {
            Vector3 prevCameraPos = cameraT.position;
            cameraT.position = Vector3.SmoothDamp(cameraT.position, cameraPoint[select] + stageToCam, ref velocity, 0.7f);
            Vector3 dif = cameraT.position - prevCameraPos;
            earthT.transform.position += dif;
            yield return null;
        }

    }

    IEnumerator MoveUfo()
    {
        inputtable = false;
        float time = 0;
        StartCoroutine("PutOnStage", false);
        StartCoroutine("PutOnStage", true);
        while (time <= 1)
        {
            time += Time.deltaTime * 2;
            ufoT.transform.position = Vector3.Lerp(stagePointsObj[prevSelect].transform.position, stagePointsObj[select].transform.position, time) + stageToUfo;
            yield return null;
        }

        inputtable = true;

    }

}
