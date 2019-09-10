using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect_Back : MonoBehaviour
{
    RectTransform rect;
    Image image;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(0.7f);
        float t = 0;
        Color defColor = image.color;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            rect.localPosition = Vector3.Lerp(Vector3.right * -450f, Vector3.right * -650f, t);
            image.color = Color.Lerp(defColor, Color.clear, t / 2f);
            yield return null;
        }
        yield break;
    }
}
