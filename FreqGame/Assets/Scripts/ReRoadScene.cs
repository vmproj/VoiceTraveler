using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReRoadScene : MonoBehaviour
{

    public void LoadScene()
    {
        StartCoroutine(SceneChange.ChangeScene(SceneManager.GetActiveScene().name));
    }
}
