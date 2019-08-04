using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [SerializeField]
    GameObject firePrefab;
    [SerializeField]
    Transform shotPosT;
    float fireSpeed = 5f;
    float interval = 3.5f;
    Transform player;
    LazerAnimation lazer;
    [SerializeField]
    GameObject expPrefab;
    [SerializeField]
    AudioClip se;
    StageController stageController;
    public bool Dead { get { return dead; } }
    bool dead;
    bool isActivized;
    ColliderObserver trigger;
    Vector3 pos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
        lazer = player.Find("Lazer").GetComponent<LazerAnimation>();
        trigger = GameObject.Find("DragonObserver").GetComponent<ColliderObserver>();
        trigger.TagName = "Player";
        pos = new Vector3(5.5f, -1.5f, 0);
    }

    void Update()
    {
        if (lazer.ShootingBeam && !Dead) StartCoroutine(Die());
        if (trigger.Hit && !isActivized)
        {
            isActivized = true;
            StartCoroutine(Activize());
        }

    }
    IEnumerator Activize()
    {
        Vector3 defPos = transform.position;
        defPos.x -= 5f;
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(defPos, pos, t);
            yield return null;
        }

        StartCoroutine(ShotFire());
    }


    IEnumerator Die()
    {
        dead = true;
        yield return new WaitForSeconds(0.6f);
        GetComponent<Animator>().SetTrigger("die");
        for (int i = 0; i < 4; i++)
        {
            Instantiate(expPrefab, new Vector3(transform.position.x + Random.Range(-1f, 1f), player.position.y, -0.5f), Quaternion.identity);
            yield return new WaitForSeconds(0.7f);
        }
        yield break;
    }

    IEnumerator ShotFire()
    {
        int graduarity = 0;
        yield return new WaitForSeconds(interval / 2f);
        while (true)
        {
            if (graduarity % 2 == 0)
            {
                stageController.ShotSE(se);
                GameObject fire = Instantiate(firePrefab, new Vector3(shotPosT.position.x, shotPosT.position.y, 0), Quaternion.identity);
                Vector3 shotVec = (player.transform.position - shotPosT.position).normalized;
                DragonFire df = fire.GetComponent<DragonFire>();
                df.Dir = shotVec;
                df.FireSpeed = fireSpeed;
            }
            else if (graduarity == 1)
            {
                for (int i = -1; i <= 1; i++)
                {
                    stageController.ShotSE(se);
                    GameObject fire = Instantiate(firePrefab, new Vector3(shotPosT.position.x, shotPosT.position.y, 0), Quaternion.identity);
                    float rad = (180 + i * 40f) * Mathf.Deg2Rad;
                    Vector3 shotVec = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
                    DragonFire df = fire.GetComponent<DragonFire>();
                    df.Dir = shotVec;
                    df.FireSpeed = fireSpeed;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else if (3 <= graduarity)
            {
                for (int i = -2; i <= 2; i++)
                {
                    stageController.ShotSE(se);
                    GameObject fire = Instantiate(firePrefab, new Vector3(shotPosT.position.x, shotPosT.position.y, 0), Quaternion.identity);
                    float rad = (180 + i * 30f) * Mathf.Deg2Rad;
                    Vector3 shotVec = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
                    DragonFire df = fire.GetComponent<DragonFire>();
                    df.Dir = shotVec;
                    df.FireSpeed = fireSpeed;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            graduarity++;
            if (Dead) yield break;
            yield return new WaitForSeconds(interval);
        }
    }
}
