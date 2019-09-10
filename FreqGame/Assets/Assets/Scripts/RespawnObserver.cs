using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//これを複数配置すれば、リスポーン場所になる
public class RespawnObserver : MonoBehaviour {

    [SerializeField, Tooltip("リスポーンする場所")] Vector3 position;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            RespawnManager.Instance.PositionRegister(position);
    }
}
