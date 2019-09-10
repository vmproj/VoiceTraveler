using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Frequency;

public class ToGame : MonoBehaviour
{

    [SerializeField] private WaveRecog waveRecog;
    private JsonProcess jsonProcess;

    private void Start()
    {
        jsonProcess = GameObject.FindObjectOfType<JsonProcess>();
    }
    public void InGameChange()
    {
        if (waveRecog.minFreq < waveRecog.maxFreq)
            jsonProcess.FreqChange(waveRecog.minFreq, waveRecog.maxFreq);
        StartCoroutine(SceneChange.ChangeScene("StageSelection"));
    }
}
