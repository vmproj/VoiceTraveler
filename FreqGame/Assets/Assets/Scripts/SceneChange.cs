using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public static IEnumerator ChangeScene(string nextSceneName)
    {

        float time = 1;
        Image image = GameObject.Find("SceneChange").GetComponent<Image>();
        while (true)
        {
            time -= Time.deltaTime / 1;
            image.material.SetFloat("_Range", time);
            if (time < 0)
            {
                //GameStateManagerにゲーム状態を保存
                if (nextSceneName.Contains("Title"))
                {
                    GameStateManager.Instance.ChangeSceneName(GameStateManager.State.Title);
                    //スタート位置へ
                    RespawnManager.Instance.isRespawn = false;
                    if (BGM.Instance != null)
                        Destroy(BGM.Instance.gameObject);
                }
                else if (nextSceneName.Contains("Game 1-2"))
                {
                    GameStateManager.Instance.ChangeSceneName(GameStateManager.State.Game2);
                    GameStateManager.Instance.Score = 0f;
                }
                else if (nextSceneName.Contains("Game 1-3"))
                {
                    GameStateManager.Instance.ChangeSceneName(GameStateManager.State.Game3);
                    GameStateManager.Instance.Score = 0f;
                }
                else if (nextSceneName.Contains("Game 1-4"))
                {
                    GameStateManager.Instance.ChangeSceneName(GameStateManager.State.Game4);
                    GameStateManager.Instance.Score = 0f;
                }
                else if (nextSceneName.Contains("Game"))
                {
                    GameStateManager.Instance.ChangeSceneName(GameStateManager.State.Game);
                    GameStateManager.Instance.Score = 0f;
                }
                else if (nextSceneName.Contains("Freq"))
                    GameStateManager.Instance.ChangeSceneName(GameStateManager.State.Frequency);
                else if (nextSceneName.Contains("Score"))
                {
                    GameStateManager.Instance.ChangeSceneName(GameStateManager.State.Score);
                    RespawnManager.Instance.isRespawn = false;
                    if (BGM.Instance != null)
                        Destroy(BGM.Instance.gameObject);
                }
                else if (nextSceneName.Contains("Stage"))
                    GameStateManager.Instance.ChangeSceneName(GameStateManager.State.StageSelection);

                TimeCount.Instance.isTimer = false;
                SceneManager.LoadScene(nextSceneName);
                yield break;
            }
            yield return null;
        }
    }
    public static IEnumerator FadeOut(float speed = 1, float delay = 0)
    {
        float time = 0;
        Image image = GameObject.Find("SceneChange").GetComponent<Image>();
        image.material.SetFloat("_Range", time);
        yield return new WaitForSeconds(delay);
        while (true)
        {
            time += Time.deltaTime * speed;
            image.material.SetFloat("_Range", time);
            if (1 <= time)
            {
                yield break;
            }
            yield return null;
        }
    }
    public static IEnumerator Quit()
    {
        float time = 1;
        Image image = GameObject.Find("SceneChange").GetComponent<Image>();
        while (true)
        {
            time -= Time.deltaTime / 2;
            image.material.SetFloat("_Range", time);
            if (time < 0)
            {
                Application.Quit();
                yield break;
            }
            yield return null;

        }
    }
}
