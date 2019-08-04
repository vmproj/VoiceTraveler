using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlend : MonoBehaviour
{

    [SerializeField] private float fadeValue;
    private Image image;
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(BlendImage());
    }

    private IEnumerator BlendImage()
    {
        while (image.color.a < 255)
        {
            image.color += Color.black * fadeValue;
            yield return null;
        }
        yield break;
    }
}
