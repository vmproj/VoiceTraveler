using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InObstacleChecker : MonoBehaviour
{

    [SerializeField] SpriteRenderer[] spriteRenders;
    [SerializeField] GameObject m2Gimmick;
    GateObserver gateObserver;
    private void Start()
    {
        m2Gimmick = GameObject.FindGameObjectWithTag("m2Gimmick");
        gateObserver = transform.parent.gameObject.GetComponent<GateObserver>();
        spriteRenders = m2Gimmick.GetComponentsInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        if (gateObserver.Opened)
            SetSkelton();
    }

    void SetSkelton()
    {
        foreach (var i in spriteRenders)
        {
            i.color = new Color(255f, 255f, 255f, 0.15f);
            Destroy(i.gameObject.GetComponent<BoxCollider2D>());
        }
    }
}
