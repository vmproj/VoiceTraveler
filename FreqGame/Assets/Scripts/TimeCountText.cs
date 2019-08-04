using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountText : MonoBehaviour {

    private Text timeText;
    void Start()
    {
        timeText = GetComponent<Text>();
        timeText.color = Color.white;
    }
	
	void Update () {
        if (TimeCount.Instance.isTimer)
        {
            if (TimeCount.Instance.remainingTime < 10f && !GameStateManager.Instance.isReload)
                timeText.color = Color.red;

            if (TimeCount.Instance.remainingTime > 0f)
            {
                
                //Time 00:00 のように表示
                timeText.text = string.Format("Time {0,3}:{1}",
                    (int)TimeCount.Instance.remainingTime, (int)((TimeCount.Instance.remainingTime - (int)TimeCount.Instance.remainingTime) * 100f));
            }
            else
                timeText.text = string.Format("Time  Over");
        }
    }
}
