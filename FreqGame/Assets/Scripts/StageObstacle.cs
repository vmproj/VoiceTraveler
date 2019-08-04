using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//障害物を管理し、親のゲート情報によって処理される
public class StageObstacle : MonoBehaviour
{

    GateObserver gateObserver;
    Collider2D gateTrigger;
    private void Start()
    {
        gateObserver = transform.parent.gameObject.GetComponent<GateObserver>();
        gateTrigger = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (gateObserver.Opened)
            Destroy(gateTrigger);
    }

}
