using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//測定シーンの状態遷移を行う
public class FreqManager : MonoBehaviour
{
    [SerializeField]
    AudioClip[] SE;
    AudioSource aud;

    [SerializeField] GameObject[] firstObjects;
    [SerializeField] GameObject[] secondObjects;
    GameObject selectObject;
    [SerializeField] GameObject cursor;
    RectTransform cursorT;
    Vector3[] cursorPos = new Vector3[2];
    [SerializeField] Animator[] animators;
    private ToGame toGame;
    RectTransform[] f_rect = new RectTransform[2];
    Image[] f_image = new Image[2];
    Vector3[] defFirstPos = new Vector3[2];
    enum Freq { measure, game };
    Freq freq;
    float t = 0;

    //測定画面かどうか
    bool isMeasure;
    bool afterStart;

    private void Start()
    {
        StartCoroutine(SceneChange.FadeOut());
        aud = GetComponent<AudioSource>();
        selectObject = firstObjects[0];
        toGame = GameObject.Find("SceneChange").GetComponent<ToGame>();
        animators[1].speed = 0f;

        cursorT = cursor.GetComponent<RectTransform>();
        cursorPos[0] = cursorT.localPosition;
        cursorPos[1] = cursorPos[0];
        cursorPos[1].y -= 120f;
        for (int i = 0; i < 2; i++)
        {
            f_rect[i] = firstObjects[i].GetComponent<RectTransform>();
            f_image[i] = firstObjects[i].GetComponent<Image>();
            defFirstPos[i] = f_rect[i].localPosition;
        }


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z))
            Choice();
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isMeasure)
            {
                aud.PlayOneShot(SE[0]);
                if (selectObject == firstObjects[0])
                {
                    selectObject = firstObjects[1];
                    animators[0].speed = 0f;
                    animators[1].speed = 1f;
                    freq = Freq.game;
                }
                else
                {
                    selectObject = firstObjects[0];
                    animators[0].speed = 1f;
                    animators[1].speed = 0f;
                    freq = Freq.measure;
                }
            }
        }
        if (freq == Freq.measure)
        {
            t = Mathf.Clamp01(t + Time.deltaTime * 5f);
        }
        else if (freq == Freq.game)
        {
            t = Mathf.Clamp01(t - Time.deltaTime * 5f);
        }
        cursorT.localPosition = Vector3.Lerp(cursorPos[1], cursorPos[0], t);
        f_rect[0].localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.8f, 1 - t);
        f_rect[1].localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.8f, t);
        f_image[0].color = Color.Lerp(Color.white, Color.gray, 1 - t);
        f_image[1].color = Color.Lerp(Color.white, Color.gray, t);
        f_rect[0].localPosition = defFirstPos[0] + Vector3.up * Mathf.Sin(Time.time * 450 * Mathf.Deg2Rad) * (t) * 5f;
        f_rect[1].localPosition = defFirstPos[1] + Vector3.up * Mathf.Sin(Time.time * 450 * Mathf.Deg2Rad) * (1 - t) * 5f;


        if (isMeasure) secondObjects[0].GetComponent<RectTransform>().localPosition = defFirstPos[1] + Vector3.up * Mathf.Sin(Time.time * 450 * Mathf.Deg2Rad) * (t) * 5f + Vector3.down * 35f;
    }

    //選択
    void Choice()
    {
        //測定画面でゲームを押したら
        if (isMeasure && !afterStart)
        {
            aud.PlayOneShot(SE[1]);
            afterStart = true;
            StartCoroutine(ButtonRemove(secondObjects[0].GetComponent<RectTransform>(), 2));
            secondObjects[1].SetActive(false);

            toGame.InGameChange();
        }
        else if(!isMeasure)
        {
            aud.PlayOneShot(SE[1]);
            StartCoroutine(ButtonRemove(f_rect[0], 0));
            StartCoroutine(ButtonRemove(f_rect[1], 1));

            //測定シーンへ
            if (selectObject == firstObjects[0])
            {
                cursor.SetActive(false);
                isMeasure = true;
                foreach (var i in secondObjects)
                {
                    i.SetActive(true);
                }
                selectObject = secondObjects[0];
            }
            //ゲームへ
            else
            {
                toGame.InGameChange();
            }
        }
    }

    //UI(ボタン)を…
    IEnumerator ButtonRemove(RectTransform r, int dir)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 5f;
            switch (dir)
            {
                case 0:
                    r.localScale = Vector3.one + (Vector3.down + Vector3.right / 2f) * t;
                    break;
                case 1:
                    r.localScale = Vector3.one + (Vector3.down + Vector3.right / 2f) * t;
                    break;
                default:
                    r.localScale = Vector3.one + (Vector3.down + Vector3.right / 2f) * t;
                    break;
            }
            yield return null;
        }
        r.gameObject.SetActive(false);
        yield break;
    }
}
