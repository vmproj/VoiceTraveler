using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//リスポーンする場所を指定するクラス
public class RespawnManager : SingletonMonoBehaviour<RespawnManager>
{
    public bool isRespawn = false;
    public Vector3 respawnPosition;
   
    private playerController playerController;
    public void Awake()
    {

        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void PositionRegister(Vector3 respawnPosition)
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
        //ゲームオーバー状態のときには更新しないようにする
        if (playerController.canControll)
        {
            this.respawnPosition = respawnPosition;
            isRespawn = true;
        }
    }
   
}
