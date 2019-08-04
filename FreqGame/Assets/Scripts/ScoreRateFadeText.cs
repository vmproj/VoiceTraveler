using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRateFadeText : FadeText {

    public override void ScoreDisplay(string point)
    {
        text.color += Color.black;
        StartCoroutine(SetFade(fadeValue));
        text.text = string.Format("×{0}.0", point);
    }
}
