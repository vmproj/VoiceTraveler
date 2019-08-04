using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplay : MonoBehaviour
{

    [SerializeField]JuliusSample juliusSample;
    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    private void LateUpdate()
    {
        if (juliusSample != null)
        {
            var match = Regex.Match(juliusSample.lastResult, @"<s>,[\w]+");
            if (match.Success)
                text.text = match.ToString().Split(',')[1];
        }
        else
        {
            text.text = "";
        }
    }
    public void Display()
    {
        if (text.color.a == 0)
            text.color += Color.black;
        else
            text.color -= Color.black;
    }
    public void FetchJulius()
    {
        juliusSample = GameObject.FindGameObjectWithTag("Julius").GetComponent<JuliusSample>();
    }

}
