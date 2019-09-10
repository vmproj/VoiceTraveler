using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerAnimation : MonoBehaviour
{
    ParticleSystem ps_part;
    ParticleSystem ps_ring;
    Transform lazer;
    Vector3 defScale;
    bool shootingBeam;
    public bool ShootingBeam
    {
        get { return shootingBeam; }
    }

    AudioSource audioSource;
    void Start()
    {
        ps_part = transform.Find("part").GetComponent<ParticleSystem>();
        ps_ring = transform.Find("ring").GetComponent<ParticleSystem>();
        lazer = transform.Find("lazer");
        defScale = lazer.transform.localScale;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
    }

    public IEnumerator  Shot(float time)
    {
        ps_part.Play();
        ps_ring.Play();
        float timeCounter = 0;
        lazer.gameObject.SetActive(true);
        while (timeCounter < time)
        {
            timeCounter += Time.deltaTime;
            while (lazer.localScale.y < 1.7f)
            {
                lazer.transform.localScale += Vector3.up * Time.deltaTime * 3f;
                yield return null;
            }
            yield return new WaitForSeconds(time);
            while (0 < lazer.localScale.y)
            {
                lazer.transform.localScale -= Vector3.up * Time.deltaTime * 2f;
                yield return null;
            }
            ps_part.Stop();
            ps_ring.Stop();
            lazer.transform.localScale = defScale;
            lazer.gameObject.SetActive(false);
            shootingBeam = false;
            yield return new WaitForSeconds(1f);
            GameObject.Find("Dragon_StopPoint").SetActive(false);
            yield break;
        }

    }

    //音声認識から呼び出される
    public void VoiceBeam()
    {
        shootingBeam = true;
        StartCoroutine(Shot(2f));
        audioSource.Play();
    }
}
