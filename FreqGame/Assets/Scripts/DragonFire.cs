using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
    bool hit;
    GameObject player;
    StageController stageController;
    playerController playerController;
    [SerializeField]
    GameObject exp;
    Dragon dragon;
    [SerializeField]
    AudioClip se;


    public float FireSpeed { get; set; }
    public Vector3 Dir { get; set; }
    SkinnedMeshRenderer skinnedMeshRenderer;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        dragon = GameObject.Find("dragon").GetComponent<Dragon>();
        playerController = player.GetComponent<playerController>();
        stageController = GameObject.FindObjectOfType<StageController>();
        skinnedMeshRenderer = GameObject.FindWithTag("PlayerSkinn").GetComponent<SkinnedMeshRenderer>();
        StartCoroutine(MoveFire());
    }
    IEnumerator MoveFire()
    {
        float t = 0;
        while (-15 < transform.position.x)
        {
            t += Time.deltaTime;
            float s = (t < 0.1f) ? FireSpeed * 6f : FireSpeed;
            transform.Translate(Dir * Time.deltaTime * s, Space.World);
            if (dragon.Dead)
            {
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
        Destroy(gameObject);
        yield break;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "ColliderForM2")
        {
            stageController.isMoving = false;
            playerController.Burned = true;
            StartCoroutine(BurnPlayerUp());
            Destroy(player.GetComponent<BoxCollider2D>());
            Instantiate(exp, transform.position, Quaternion.identity);
            GetComponent<ParticleSystem>().Stop();
            Destroy(gameObject, 2.5f);
            hit = true;
        }
    }
    IEnumerator BurnPlayerUp()
    {
        stageController.ShotSE(se);
        float t = 0;
        Vector3 defPos = player.transform.position;
        while (t <= 1f)
        {
            t += Time.deltaTime / 4f;
            skinnedMeshRenderer.materials[0].color = Color.Lerp(Color.white, Color.black, t * 2f);
            player.transform.position = Vector3.Lerp(defPos, defPos + Vector3.down * 15f, t);
            yield return null;
        }
        yield break;
    }

}
