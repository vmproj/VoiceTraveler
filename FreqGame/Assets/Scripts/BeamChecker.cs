using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamChecker : MonoBehaviour
{
    bool flg = false;
    GateObserver gateObserver;
    [SerializeField] LazerAnimation lazerAnimation;
    void Start()
    {
        gateObserver = GetComponent<GateObserver>();
    }

    void Update()
    {
        if (gateObserver.Opened && !flg)
        {
            lazerAnimation.VoiceBeam();
            flg = true;
        }
    }
}
