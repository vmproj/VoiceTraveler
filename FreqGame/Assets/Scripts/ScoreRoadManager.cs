using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRoadManager : MonoBehaviour {

    [SerializeField]FadeText fadeText;
    [SerializeField]UpGameObject upGameObject;
    private void Awake()
    {
        fadeText = GetComponent<FadeText>();
        upGameObject = GetComponent<UpGameObject>();
    }
    private void OnEnable()
    {
        StartCoroutine(fadeText.SetFade(0.05f));
        upGameObject.UpStart();
    }
}
