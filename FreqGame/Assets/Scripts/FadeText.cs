using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeText : MonoBehaviour
{
    [NonSerialized] public Text text;
    [SerializeField] protected float fadeValue;
    protected void Awake()
    {
        text = GetComponent<Text>();
    }

    public virtual void ScoreDisplay(string point)
    {
        StartCoroutine(SetFade(fadeValue));
        text.text = string.Format("+{0}P", point);
    }
    public virtual IEnumerator SetFade(float fadeValue)
    {
        
        while (text.color.a >= 0.0f)
        {
            text.color -= Color.black * fadeValue;
            yield return new WaitForSeconds(0.1f);
        }
        
        yield break;
    }
}
