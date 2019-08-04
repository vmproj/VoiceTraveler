using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//画面全体が暗くなるのを制御
public class DarkSet : MonoBehaviour
{

    [SerializeField] Renderer dark;
    [SerializeField] Transform darkTransform;
    Transform player;
    //暗くなる半径
    [SerializeField] float radius = 0.1f;
    //ゲートの情報
    GateObserver darkGateObserver;
    //暗くなっているか
    bool isShrink;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        darkGateObserver = GameObject.Find("DarkObserver").GetComponent<GateObserver>();
    }
    void Update()
    {
        if (isShrink)
        {
            float offset = player.transform.position.y + 0.5f;
            darkTransform.position = new Vector3(darkTransform.position.x,
                offset, darkTransform.position.z);
        }
        if (darkGateObserver.Opened)
            StartCoroutine(Expand());
    }

    public IEnumerator Shrink()
    {
        isShrink = true;
        float t = dark.material.GetFloat("_Cutoff");
        while (radius < t)
        {
            dark.material.SetFloat("_Cutoff", t);
            t -= 0.005f;
            yield return null;
        }
        yield break;
    }

    public IEnumerator Expand()
    {
        float t = dark.material.GetFloat("_Cutoff");
        while (1.1f > t)
        {
            dark.material.SetFloat("_Cutoff", t);
            t += 0.015f;
            yield return null;
        }
        isShrink = false;
        yield break;
    }
}
