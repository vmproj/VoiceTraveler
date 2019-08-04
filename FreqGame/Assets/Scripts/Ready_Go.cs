using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ready_Go : MonoBehaviour
{
    [SerializeField]
    Sprite[] readyGo;

    Image image;
    RectTransform rect;
    TimeCount timeCount;
    float t = 0;
    private void Start()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        timeCount = GameObject.Find("Canvas/Time").GetComponent<TimeCount>();
        StartCoroutine(ReadyGo());
    }

    private IEnumerator ReadyGo()
    {
        image.sprite = readyGo[0];
        yield return new WaitForSeconds(0.5f);
        while (!TimeCount.Instance.isTimer)
        {
            t += Time.deltaTime * 1.7f;
            image.color -= Color.black * Time.deltaTime * 1.7f;
            rect.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, t);
            yield return null;
        }
        image.sprite = readyGo[1];
        image.color = Color.white;
        t = 0;
        while (image.color.a > 0f)
        {
            t += Time.deltaTime * 1.5f;
            image.color -= Color.black * Time.deltaTime * 1.5f;
            rect.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, t);
            yield return null;
        }
        yield break;
    }
}
