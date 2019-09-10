using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    Image[] menus;
    Text[] menuText;
    Color[] buttonTextColor;
    [SerializeField]
    Text[] yesNo;
    [SerializeField]
    Image message;
    Text messageText;
    [SerializeField, Multiline]
    string[] messageString;
    [SerializeField, Multiline]
    string messageString_yesNo;

    Vector3[] defMenuPos;
    Vector3[] defYesNoPos = new Vector3[2];

    [SerializeField]
    Image cursor;
    [SerializeField]
    RectTransform[] cursorPos_menu;
    [SerializeField]
    RectTransform[] cursorPos_yesNo;

    [SerializeField]
    Image titleLogo;
    [SerializeField]
    GameObject titleLogoEffect;
    [SerializeField]
    Image back;
    [SerializeField]
    Text c;

    [SerializeField]
    GameObject[] stringsSpr;

    /// <summary>
    /// 「スタート」は０
    /// 「やめる」は１
    /// とする。
    /// </summary>
    int? selectMenu = 0;
    bool? selectYesNo = null;
    int menuCount;
    float[] t_menu;
    float[] t_yesNo = new float[2];
    Color defMenuColor = new Color(1, 1, 1, 0.5f);
    Color defYesNoColor = new Color(1, 1, 1, 0.5f);
    Vector3 selectedScale = Vector3.one * 1.2f;
    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    ParticleSystem comet;

    [SerializeField]
    LineRenderer line;
    float delay = 0.5f;

    [SerializeField]
    AudioClip[] se;
    AudioSource audioSource;

    void Start()
    {
        StartCoroutine(GenerateAndManageStrings());
        messageText = message.GetComponentInChildren<Text>();
        menuCount = menus.Length;
        t_menu = new float[menuCount];
        menuText = new Text[menuCount];
        buttonTextColor = new Color[menuCount];
        defMenuPos = new Vector3[menuCount];
        message.color = Color.white - Color.black;
        cursor.color = Color.white - Color.black;
        cursor.rectTransform.position = cursorPos_menu[0].position;
        titleLogo.color = Color.white - Color.black;
        titleLogoEffect.GetComponent<Image>().color = Color.white - Color.black;
        for (int i = 0; i < menuCount; i++)
        {
            defMenuPos[i] = menus[i].rectTransform.position;
            menus[i].color = Color.white - Color.black;
            menuText[i] = menus[i].GetComponentInChildren<Text>();
            buttonTextColor[i] = menuText[i].color;
            menuText[i].color = Color.white - Color.black;
        }
        for (int i = 0; i < 2; i++)
        {
            defYesNoPos[i] = yesNo[i].rectTransform.position;
            yesNo[i].color = Color.white - Color.black;
        }

        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SceneChange.FadeOut(2));
        StartCoroutine(EnableMenu());
    }

    void Update()
    {


    }

    IEnumerator GenerateAndManageStrings()
    {
        int stringsLength = stringsSpr.Length;
        int stringsCount = 70;
        Transform[] stringsT = new Transform[stringsCount];
        float[] speed = new float[stringsCount];
        for (int i = 0; i < stringsCount; i++)
        {
            GameObject s = Instantiate(stringsSpr[Random.Range(0, stringsLength)]);
            s.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, 0.7f);
            stringsT[i] = s.transform;
            stringsT[i].Translate(Random.Range(-2f, 0), Random.Range(-3f, 4f), 0);
            speed[i] = Random.Range(0.5f, 3f);
        }
        while (true)
        {
            for (int i = 0; i < stringsCount; i++)
            {
                stringsT[i].Translate(speed[i] * Time.deltaTime, 0, 0);
                if (6.7f < stringsT[i].position.x)
                {
                    stringsT[i].Translate(-13, 0, 0);
                }
            }
            yield return null;
        }

    }

    IEnumerator FadeInImage(Image image, Color goal, float speed)
    {
        float t = 0;
        Color def = image.color;
        while (t <= 1)
        {
            t += Time.deltaTime * speed;
            image.rectTransform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, t);
            image.color = Color.Lerp(def, goal, t);
            yield return null;
        }
        yield break;
    }
    IEnumerator FadeInText(Text text, Color goal, float speed)
    {
        float t = 0;
        Color def = text.color;
        while (t <= 1)
        {
            t += Time.deltaTime * speed;
            text.color = Color.Lerp(def, goal, t);
            yield return null;
        }
        yield break;
    }
    bool afterSelect;
    IEnumerator EnableMenu()
    {
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(FadeInImage(titleLogo, Color.white, 0.8f));
        StartCoroutine(FadeInImage(back, new Color(1, 1, 1, 0.8f), 1));
        StartCoroutine(FadeInText(c, Color.white, 1));
        StartCoroutine(TitleLogoEffect());
        comet.Play();
        StartCoroutine(MoveLine());
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < menuCount; i++)
        {
            StartCoroutine(FadeInImage(menus[i], defMenuColor, 1.3f));
            StartCoroutine(FadeInText(menuText[i], buttonTextColor[i], 1.3f));
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeInImage(cursor, Color.white, 2));
        StartCoroutine(FadeInImage(message, Color.white, 2));
        StartCoroutine(TrembleCamera());
        yield return new WaitForSeconds(0.3f);
        while (true)
        {
            StartCoroutine("ShowMenuMessage");

            if (Input.GetButtonDown("Vertical") && !afterSelect)
            {
                audioSource.PlayOneShot(se[0]);
                selectMenu += (int)Input.GetAxisRaw("Vertical");
                if (selectMenu < 0) selectMenu = menuCount - 1;
                if (menuCount <= selectMenu) selectMenu = 0;
                StopCoroutine("ShowMenuMessage");
                StartCoroutine("ShowMenuMessage");
                StopCoroutine("MoveCursor");
                StartCoroutine("MoveCursor", cursorPos_menu[(int)selectMenu].position);
            }
            AnimateMenu();
            if ((Input.GetKeyDown(KeyCode.Z) || (Input.GetKeyDown(KeyCode.Return))))
            {
                switch (selectMenu.Value)
                {
                    case 0:
                        audioSource.PlayOneShot(se[1]);
                        StopCoroutine("ShowMessage");
                        StopCoroutine("ShowMenuMessage");
                        messageText.text = string.Empty;
                        selectMenu = null;
                        if (!afterSelect)
                        {
                            afterSelect = true;
                            StartCoroutine(EnableYesNo());
                        }
                        break;
                    case 1:
                        if (!afterSelect)
                        {
                            audioSource.PlayOneShot(se[1]);
                            afterSelect = true;
                            StartCoroutine(SceneChange.Quit());
                        }
                        break;
                }
            }
            if (!selectMenu.HasValue) yield break;
            yield return null;
        }
    }
    IEnumerator TitleLogoEffect()
    {
        yield return new WaitForSeconds(1.2f);
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / 1f;
            titleLogoEffect.GetComponent<Image>().color = Color.white - Color.black * t * 2f;
            titleLogoEffect.GetComponent<RectTransform>().localScale = Vector3.one * (t + 1f);
            yield return null;
        }
        yield break;
    }

    IEnumerator EnableYesNo()
    {
        bool afterSelect = false;
        selectYesNo = true;
        StartCoroutine("MoveCursor", cursorPos_yesNo[(bool)selectYesNo ? 0 : 1].position);
        StartCoroutine(ShowYesNoMessage());
        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(FadeInText(yesNo[i], defYesNoColor, 5));
        }
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
            if ((Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")) && !afterSelect)
            {
                audioSource.PlayOneShot(se[0]);
                selectYesNo = !selectYesNo;
                StopCoroutine("MoveCursor");
                StartCoroutine("MoveCursor", cursorPos_yesNo[(bool)selectYesNo ? 0 : 1].position);
            }
            if ((Input.GetKeyDown(KeyCode.Z) || (Input.GetKeyDown(KeyCode.Return))) && !afterSelect)
            {
                audioSource.PlayOneShot(se[1]);
                if ((bool)selectYesNo) StartCoroutine(SceneChange.ChangeScene("Frequency"));
                else StartCoroutine(SceneChange.ChangeScene("StageSelection"));
                afterSelect = true;
            }
            AnimateYesNo();

            yield return null;
        }

    }

    IEnumerator MoveCursor(Vector3 goal)
    {
        Vector3 def = cursor.rectTransform.position;
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime * 10;
            cursor.rectTransform.position = Vector3.Lerp(def, goal, t);
            yield return null;
        }
        yield break;
    }

    void AnimateMenu()
    {
        for (int i = 0; i < menuCount; i++)
        {
            t_menu[i] += (i == selectMenu) ? Time.deltaTime * 6 : -Time.deltaTime * 4;
            t_menu[i] = Mathf.Clamp01(t_menu[i]);
        }
        for (int i = 0; i < menuCount; i++)
        {
            menus[i].color = Color.Lerp(defMenuColor, Color.white, t_menu[i]);
            menus[i].rectTransform.localScale = Vector3.Lerp(Vector3.one, selectedScale, t_menu[i]);
            if (i == selectMenu)
            {
                menus[i].rectTransform.position = new Vector3(defMenuPos[i].x, defMenuPos[i].y + Mathf.Sin(Time.time * 700 * Mathf.Deg2Rad), defMenuPos[i].z);
            }
        }
    }

    void AnimateYesNo()
    {
        for (int i = 0; i < 2; i++)
        {
            t_yesNo[i] += (i == ((bool)selectYesNo ? 0 : 1)) ? Time.deltaTime * 6 : -Time.deltaTime * 4;
            t_yesNo[i] = Mathf.Clamp01(t_yesNo[i]);
        }
        for (int i = 0; i < 2; i++)
        {
            yesNo[i].color = Color.Lerp(defYesNoColor, Color.white, t_yesNo[i]);
            yesNo[i].rectTransform.localScale = Vector3.Lerp(Vector3.one, selectedScale, t_yesNo[i]);
            if (i == ((bool)selectYesNo ? 0 : 1))
            {
                yesNo[i].rectTransform.position = new Vector3(defYesNoPos[i].x, defYesNoPos[i].y + Mathf.Sin(Time.time * 800 * Mathf.Deg2Rad) * 0.6f, defYesNoPos[i].z);
            }
        }
    }

    IEnumerator ShowMenuMessage()
    {
        messageText.text = string.Empty;
        for (int i = 0; i < messageString[(int)selectMenu].Length; i++)
        {
            messageText.text += messageString[(int)selectMenu][i];
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator ShowYesNoMessage()
    {
        messageText.text = string.Empty;
        for (int i = 0; i < messageString_yesNo.Length; i++)
        {
            messageText.text += messageString_yesNo[i];
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }

    IEnumerator MoveLine()
    {
        line.gameObject.SetActive(true);
        line.SetPosition(0, new Vector3(-400, 0, 0));
        line.SetPosition(line.positionCount - 1, new Vector3(400, 0, 0));
        while (true)
        {
            for (int i = 1; i < line.positionCount - 1; i++)
            {
                if (i < line.positionCount * 0.1f)
                {
                    line.SetPosition(i, new Vector3(i * 7.7f - 380, Random.Range(-5, 5), 0));
                }
                else if (line.positionCount * 0.1f < i && i < line.positionCount * 0.13f)
                {
                    line.SetPosition(i, new Vector3(i * 7.7f - 380, Random.Range(-110, 110), 0));
                }
                else if (line.positionCount * 0.2f < i && i < line.positionCount * 0.3f)
                {
                    line.SetPosition(i, new Vector3(i * 7.7f - 380, 0, 0));
                }
                else if (line.positionCount * 0.4f < i && i < line.positionCount * 0.58f)
                {
                    line.SetPosition(i, new Vector3(i * 7.7f - 380, Random.Range(-10, 10), 0));
                }
                else if (line.positionCount * 0.66f < i && i < line.positionCount * 0.7f)
                {
                    line.SetPosition(i, new Vector3(i * 7.7f - 380, 0, 0));
                }
                else if (line.positionCount * 0.7f < i && i <= line.positionCount * 0.76f)
                {
                    line.SetPosition(i, new Vector3(i * 7.7f - 380, Random.Range(-1f, 1f) * i, 0));
                }
                else if (line.positionCount * 0.76f < i && i < line.positionCount * 0.8f)
                {
                    line.SetPosition(i, new Vector3(i * 7.7f - 380, Random.Range(-130, 100), 0));
                }
                else if (line.positionCount * 0.85f < i)
                {
                    line.SetPosition(i, new Vector3(i * 7.7f - 380, Random.Range(-5, 5), 0));
                }
                else
                {
                    line.SetPosition(i, new Vector3(i * 7.7f - 380, Random.Range(-50, 50), 0));
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator TrembleCamera()
    {
        Vector3 defPos = cameraTransform.position;
        Vector3 startPos = defPos;
        Vector3 goalPos = defPos;
        Vector3 startRot = Vector3.zero;
        Vector3 goalRot = Vector3.zero;
        float range = 0.4f;
        while (true)
        {
            float time = 0;
            float speed = Random.Range(0.5f, 1f);
            goalPos.x = defPos.x + Random.Range(-range, range);
            goalPos.y = defPos.y + Random.Range(-range, range);
            goalRot.z = Random.Range(-range * 7, range * 7);
            startPos = cameraTransform.position;
            startRot = cameraTransform.rotation.eulerAngles;
            while (time < 1)
            {
                time += Time.deltaTime * speed;
                cameraTransform.position = Vector3.Slerp(startPos, goalPos, time);
                cameraTransform.rotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(goalRot), time);
                yield return null;
            }
            yield return null;
        }
    }
}
