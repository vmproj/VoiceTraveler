using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedText : MonoBehaviour {

    private Text speedText;
    private StageController stageController;

    private float initSpeed;
    private void Start()
    {
        speedText = GetComponent<Text>();
        stageController = GameObject.FindObjectOfType<StageController>();
        initSpeed = stageController.lowSpeed;
    }
    private void Update()
    {
        if (stageController.isMoving)
            SpeedTextSet(((int)(stageController.GetStageSpeed() / initSpeed * 100)).ToString());
        else
            SpeedTextSet("0");
    }
    public void SpeedTextSet(string speed)
    {
        speedText.text = string.Format("<color=#ffaaaa>Speed</color> {0,3}<color=#bbbbbb>%</color>", speed);
    }
}
