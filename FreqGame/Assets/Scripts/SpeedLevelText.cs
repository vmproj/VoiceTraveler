using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedLevelText : MonoBehaviour
{

    private Text speedLevelText;
    private StageController stageController;

    private float minAcc,maxAcc;
    public int speed_Level;
    private void Start()
    {
        speedLevelText = GetComponent<Text>();
        stageController = GameObject.FindObjectOfType<StageController>();
        minAcc = stageController.minAcc;
        maxAcc = stageController.maxAcc;
    }
    private void Update()
    {
        speed_Level = (int)((stageController.GetStageAcc() - minAcc) * 5f / (maxAcc - minAcc));
        SpeedLevelTextSet(speed_Level.ToString());
    }
    public void SpeedLevelTextSet(string Level)
    {
        speedLevelText.text = string.Format("<color=#ccff99>Speed</color> <color=#bbbbbb>Lv.</color>{0}", Level);
    }
}