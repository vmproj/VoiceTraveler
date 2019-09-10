using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ゲーム状態を管理する
public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{

    public enum State { Frequency, Game, Game2, Game3, Game4, Score, Title, StageSelection };
    public float Score = 0f;
    public State state;
    public State prestate;
    public bool isReload = false;

    public void Awake()
    {

        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        SetActiveScene(SceneManager.GetActiveScene().name);
        prestate = State.Title;
    }

    private void Update()
    {
        //タイトルに戻る
        if (Input.GetKeyDown(KeyCode.T))
        {
            //名前登録シーンでなければ
            if (this.state != State.Score)
                StartCoroutine(SceneChange.ChangeScene("Title"));
        }
        //ステージの先頭に戻る(スコア・タイム・中間ポイントリセット)
        else if (Input.GetKeyDown(KeyCode.R))
        {
            //ゲームシーンであれば
            if (SceneManager.GetActiveScene().name.Contains("Game"))
            {
                isReload = true;
                RespawnManager.Instance.isRespawn = false;
                StartCoroutine(SceneChange.ChangeScene(SceneManager.GetActiveScene().name));
            }
        }
    }
    public void ChangeSceneName(State state)
    {
        prestate = this.state;
        this.state = state;
    }

    public bool isSameState()
    {
        if (state == prestate)
            return true;
        return false;
    }

    private void SetActiveScene(string sceneName)
    {
        //GameStateManagerにゲーム状態を保存
        if (sceneName.Contains("Title"))
            this.state = State.Title;
        else if (sceneName.Contains("Game 1-2"))
            state = State.Game2;
        else if (sceneName.Contains("Game 1-3"))
            state = State.Game3;
        else if (sceneName.Contains("Game 1-4"))
            state = State.Game4;
        else if (sceneName.Contains("Game"))
            state = State.Game;
        else if (sceneName.Contains("Freq"))
            state = State.Frequency;
        else if (sceneName.Contains("Score"))
            state = State.Score;
        else if (sceneName.Contains("Stage"))
            state = State.StageSelection;
    }
}
