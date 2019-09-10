using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ゲーム中に、同じシーンなら継続して流す
//Reloadや時間切れのときはBGMは最初から
//シーンが変わるときはこのゲームオブジェクトを破棄
public class BGM : SingletonMonoBehaviour<BGM>
{
    public void Awake()
    {
        if (this != Instance)
        {
            //リロードしてないかつ時間切れでないとき、継続
            if (!GameStateManager.Instance.isReload && !TimeCount.Instance.isTimeOver)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Destroy(Instance.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
