using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    [SerializeField]
    AudioClip se;

    float movement = 0;
    StageController stageController;
    ColliderObserver upperCollider;
    ColliderObserver lowerCollider;
    ColliderObserver backCollider;

    float movementRange = 10f;//移動範囲（上下の）。
    float smoothTime = 0.4f;//どれくらいシャキッと動くか。
    float maxSpeed = 7f;
    Vector3 target;
    Vector3 beforePos;

    public bool canControll;
    public bool Dead { get; set; }

    Image gameOver;

    RectTransform movementMask;
    LandingChecker landingChecker;
    public float Movement
    {
        get
        {
            return movement;
        }
        set
        {
            movement = Mathf.Clamp01(value);
        }



    }

    public bool Burned { get; set; }

    Vector3 velocity = Vector3.zero;
    void Start()
    {

        stageController = GameObject.Find("StageController").GetComponent<StageController>();
        upperCollider = transform.GetChild(0).gameObject.GetComponent<ColliderObserver>();
        lowerCollider = transform.GetChild(1).gameObject.GetComponent<ColliderObserver>();
        backCollider = transform.GetChild(2).gameObject.GetComponent<ColliderObserver>();

        canControll = true;

        gameOver = GameObject.Find("GameOver").GetComponent<Image>();
        movementMask = GameObject.Find("MovementMask").GetComponent<RectTransform>();
        landingChecker = GetComponentInChildren<LandingChecker>();

        TimeCount.Instance.isTimerStart();
    }

    void Update()
    {
        movementMask.sizeDelta = new Vector2(70, Mathf.Round(Movement * 20) * 60);

        if ((transform.position.y < -7 || Burned) && !Dead)
        {
            StartCoroutine(Die());
        }
        if (canControll && TimeCount.Instance.isTimer)
        {
            Move();
        }
        if (Dead) stageController.isMoving = false;
        GameObject.Find("Height").GetComponent<Text>().text = "Height " + target.y.ToString("F1");
    }
    void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow) & !upperCollider.Hit)
        {
            Movement += Time.deltaTime;
        }
        if (!lowerCollider.Hit)
        {
            movement -= Time.deltaTime * 0.5f;//重力
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            //movement -= Time.deltaTime;
        }
        //transform.position = new Vector3(-5, (Movement - 0.5f) * 10, 0);
        target = new Vector3(transform.position.x, (Movement - 0.5f) * movementRange, 0);
        beforePos = transform.position;

        if (upperCollider.Hit & transform.position.y < target.y)
        {
            stageController.ShotSE(se);
            velocity = Vector3.zero;
            transform.position = beforePos;
            target.y = transform.position.y;
            Movement = target.y / movementRange + 0.5f;
        }
        else if (lowerCollider.Hit & target.y < transform.position.y)
        {
            velocity = Vector3.zero;
            transform.position = beforePos;
            target.y = transform.position.y;
        }
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, maxSpeed);
    }


    public IEnumerator Die()
    {
        Dead = true;
        canControll = false;
        float time = 0;
        while (true)
        {
            stageController.isMoving = false;
            time += Time.deltaTime / 2f;
            //このコルーチンは時間切れや落下などいろいろなところで呼ばれているので
            //排他処理をする
            if (gameOver != null)
            {
                gameOver.color = (Color.white * time);
                if (time >= 1)
                {
                    StartCoroutine(SceneChange.ChangeScene(SceneManager.GetActiveScene().name));
                    yield break;
                }
            }
            yield return null;
        }




    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Map")
        {

            stageController.PlaySE(se);
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Map")
        {
            if (!landingChecker.IsHitThisTag("Map"))
                stageController.isMoving = true;
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        //M2クラスの場合は判定を無視
        if (collider.gameObject.GetComponent<M2>() == null)
        {
            //ギミックブロックの場合は無視する
            if (collider.gameObject.tag == "Map" && collider.gameObject.GetComponent<CollisionGimmick>() == null)
            {

                if(!backCollider.Hit) stageController.isMoving = false;//自機が触れている対象がM2でもギミックでもない場合は動かない。
            }
            else
                stageController.isMoving = true;
        }
        else
        {
            //stageController.isMoving = false;
        }
    }


}
