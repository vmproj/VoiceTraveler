using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲームオブジェクトを上に動かすだけのクラス。主にテキスト
public class UpGameObject : MonoBehaviour
{

    [SerializeField, Tooltip("上向きのスピード")] private float upSpeed;
    private Transform trans;
    private Vector3 move;

    private FadeText fadeText;
    private void Awake()
    {
        trans = this.gameObject.transform;
        move = new Vector3(0f, upSpeed, 0f);
        fadeText = GetComponent<FadeText>();
    }
    public void UpStart()
    {
        StartCoroutine(UpObject());
    }
    private IEnumerator UpObject()
    {
        while (fadeText.text.color.a > 0f)
        {
            trans.position += move * Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
