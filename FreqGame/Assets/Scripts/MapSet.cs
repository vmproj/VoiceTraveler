using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSet : MonoBehaviour {

	void Start () {
        //リスポーン位置があれば
        if (RespawnManager.Instance.isRespawn)
            RespawnSet();
        else
            GameStateManager.Instance.Score = 0f;
    }
    public void RespawnSet()
    {
        this.transform.position = RespawnManager.Instance.respawnPosition;
    }
}
