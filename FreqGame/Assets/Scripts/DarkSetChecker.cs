using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSetChecker : MonoBehaviour {

    private DarkSet darkSet;
    private void Start()
    {
        darkSet = GameObject.Find("DarkSet").GetComponent<DarkSet>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(darkSet.Shrink());
        }
    }
}
