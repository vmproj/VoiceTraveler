using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M2 : MonoBehaviour
{
    bool hit;
    GameObject player;
    SkinnedMeshRenderer skinnedMeshRenderer;
    StageController stageController;
    playerController playerController;
    [SerializeField]
    AudioClip se;
    Collider2D col;
    SpriteRenderer spr;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<playerController>();
        skinnedMeshRenderer = GameObject.FindWithTag("PlayerSkinn").GetComponent<SkinnedMeshRenderer>();
        stageController = GameObject.FindObjectOfType<StageController>();
        col = GetComponent<Collider2D>();
        spr = GetComponent<SpriteRenderer>();
        switch (moveType)
        {
            case MoveType.dontMove:
                break;
            case MoveType.reciprocate:
                StartCoroutine(Reciprocate());
                break;
            case MoveType.circular:
                StartCoroutine(Circular());
                break;
            case MoveType.flicker:
                StartCoroutine(Flicker());
                break;
        }
    }
    public enum MoveType { dontMove, reciprocate, circular, flicker}

    [SerializeField]
    public MoveType moveType;
    [SerializeField]
    Vector2 moveDir;
    [SerializeField, Range(0f, 5f)]
    float speed_Re;
    [SerializeField, Range(-200f, 200f)]
    float speed_Cir;
    [SerializeField, Range(0f, 15f)]
    float radius;
    [SerializeField, Range(2,20)]
    int count;
    [SerializeField, Range(1f, 4f)]
    float interval;
    [SerializeField, Range(0f, 4f)]
    float delay;

    IEnumerator Reciprocate()
    {
        Vector3 defPos = transform.localPosition;
        Vector3 goalPos = defPos + new Vector3(moveDir.x, moveDir.y, 0);
        float t = 0;
        while (true)
        {
            t = Mathf.PingPong(Time.time * speed_Re, 1f);
            transform.localPosition = Vector3.Lerp(defPos, goalPos, t);
            yield return null;
        }
    }
    IEnumerator Circular()
    {
        Vector3 defPos = transform.localPosition;
        Transform[] child = new Transform[count];
        for (int i = 1; i < count; i++)
        {
            GameObject c = Instantiate(gameObject, transform.parent);
            c.GetComponent<M2>().moveType = MoveType.dontMove;
            child[i] = c.transform;
        }
        child[0] = gameObject.transform;
        while (true)
        {
            for (int i = 0; i < count; i++)
            {
                float rad = (360 / (float)count * i + Time.time * speed_Cir) * Mathf.Deg2Rad;
                child[i].localPosition = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius + defPos;
            }
            yield return null;
        }
    }
    IEnumerator Flicker()
    {
        bool isActive = false;
        yield return new WaitForSeconds(delay);
        while (true)
        {
            col.enabled = isActive;
            spr.color = (isActive) ? Color.white : Color.white - Color.black * 0.5f;
            isActive = !isActive;
            yield return new WaitForSeconds(interval);
        }
    }

    void Update()
    {
        if (hit)
        {
            //skinnedMeshRenderer.materials[0].color = (Time.time % 0.05f < 0.025f ? Color.white : Color.black);
            StartCoroutine(Damage());
            hit = false;
        }


    }
    IEnumerator Damage()
    {
        stageController.PlaySE(se);
        Texture emissionTex = skinnedMeshRenderer.materials[0].GetTexture("_EmissionMap");
        skinnedMeshRenderer.materials[0].SetTexture("_EmissionMap", null);
        bool flicker = true;
        float time = 0;
        while (time < 3f)
        {
            time += Time.deltaTime;
            skinnedMeshRenderer.materials[0].SetColor("_EmissionColor", flicker ? Color.black : Color.white * 0.7f);
            flicker = !flicker;
            yield return new WaitForSeconds(0.02f);
        }
        skinnedMeshRenderer.materials[0].SetTexture("_EmissionMap", emissionTex);
        skinnedMeshRenderer.materials[0].SetColor("_EmissionColor", Color.white);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "ColliderForM2")
        {
            stageController.isMoving = false;
            playerController.canControll = false;
            Destroy(player.GetComponent<BoxCollider2D>());
            hit = true;
            StartCoroutine(player.GetComponent<playerController>().Die());
        }
    }

}
