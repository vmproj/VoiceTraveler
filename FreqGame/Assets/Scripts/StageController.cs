using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    TimeCount timeCount;
    Transform map;
    public bool isMoving;
    public bool canMove = true;
    public readonly float lowSpeed = 1.7f, highSpeed = 3.0f;
    //周波数から求められるスピード
    [SerializeField]private float freqSpeed = 1.7f;
    [SerializeField] float proceedSpeed = 1.7f;

    //ステージにぶつからない時につく加速度
    [SerializeField] float acc = 1.0f;
    public readonly float minAcc = 1.0f,maxAcc = 2.0f;
    [SerializeField]
    RectTransform back;


    Transform cameraT;
    Transform playerT;

    AudioSource aud;

    public void PlaySE(AudioClip clip)
    {
        aud.clip = clip;
        aud.Play();
    }
    public void ShotSE(AudioClip clip)
    {
        aud.PlayOneShot(clip);
    }

    float procedure = 0;
    public float Procedure { get { return procedure; } }

    void Start()
    {
        aud = GetComponent<AudioSource>();


        timeCount = GameObject.Find("Canvas/Time").GetComponent<TimeCount>();
        StartCoroutine(SceneChange.FadeOut());

        map = GameObject.Find("map").transform;
        isMoving = true;

        GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
        foreach (GameObject g in maps)
        {
            if (g.name.Substring(0, 2) == "m1")
            {
                float color = Random.Range(0.7f, 1f);
                //g.GetComponent<SpriteRenderer>().color = new Color(color, color, color, Random.Range(0.9f, 1f));
            }
        }

        cameraT = Camera.main.transform;
        playerT = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        if (canMove)
        {
            if (TimeCount.Instance.isTimer)
            {
                procedure = Time.deltaTime * proceedSpeed * (isMoving ? 1f : 0f);
                map.position += Vector3.left * procedure;

                acc = isMoving ? acc * 1.003f : 1.0f;
                acc = Mathf.Clamp(acc, minAcc, maxAcc);
                SetStageSpeed();
                //Debug.Log("acc "+ acc + "  tmpSpeed " + freqSpeed + "  proceedSpeed " + proceedSpeed);
            }
            else
            {
                proceedSpeed = 0f;
            }
        }
        cameraT.position = new Vector3(cameraT.position.x, playerT.position.y / 10f, cameraT.position.z);
    }

    //周波数クラスから呼び出されるメソッド
    public void SetFreqSpeed(float speed)
    {
        freqSpeed = speed;
    }
    public float GetFreqSpeed()
    {
        return freqSpeed;
    }
    private void SetStageSpeed()
    {
        proceedSpeed = freqSpeed * acc;
    }
    public float GetStageSpeed()
    {
        return proceedSpeed;
    }
    public float GetStageAcc()
    {
        return acc;
    }
}
