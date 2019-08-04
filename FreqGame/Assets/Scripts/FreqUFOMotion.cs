using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Frequency;

public class FreqUFOMotion : MonoBehaviour
{
    [SerializeField]
    WaveRecog waveRecog;
    Vector3 defPos;

    void Start()
    {

        defPos = transform.position;
    }

    void Update()
    {
        transform.position = defPos + Vector3.up * Mathf.Clamp(waveRecog.DFreq / 200f, 0, 6f);

    }
}
