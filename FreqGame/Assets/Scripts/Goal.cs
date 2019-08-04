using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    [SerializeField]
    AudioClip[] se;
    StageController stageController;
    playerController playerController_I;
    Animator animator;
    Image clear_UI;
    Image clear_UI_Effect;

    Transform arrow;
    Transform[] chars = new Transform[4];

    bool cleared;

    void Start()
    {

        stageController = FindObjectOfType<StageController>();
        playerController_I = FindObjectOfType<playerController>();
        animator = GetComponent<Animator>();
        clear_UI = GameObject.Find("CLEAR_UI").GetComponent<Image>();
        clear_UI_Effect = GameObject.Find("CLEAR_UI_Effect").GetComponent<Image>();

        arrow = transform.Find("arrow");
        for (int i = 0; i < arrow.childCount; i++) chars[i] = arrow.GetChild(i);
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        for (int i = 0; i < arrow.childCount; i++)
        {
            Vector3 defPos = chars[i].localPosition;
            StartCoroutine(AnimateChar(i, defPos));
            yield return new WaitForSeconds(0.1f);
        }
        Vector3 def = arrow.localPosition;
        while (true)
        {
            arrow.localPosition = def + Vector3.right * Mathf.Sin(Time.time * 400f * Mathf.Deg2Rad) * 0.05f;
            yield return null;
        }
    }
    IEnumerator AnimateChar(int charNum, Vector3 def)
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime * 3f;
            chars[charNum].localPosition = def + Vector3.up * Mathf.Abs(Mathf.Sin(t * 180f * Mathf.Deg2Rad)) * 0.05f;
            yield return null;
            if (1f <= t)
            {
                t = 0;
                yield return new WaitForSeconds(0.5f);
            }
        }

    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !cleared)
        {
            animator.SetTrigger("Enter");
            playerController_I.canControll = false;
            stageController.canMove = false;
            StartCoroutine(AbsorbPlayer(other.transform));
        }
    }
    IEnumerator AbsorbPlayer(Transform player)
    {
        StartCoroutine(Sound());
        cleared = true;
        float t = 0;
        Vector3 defPos = player.position;
        Vector3 defScale = player.localScale;
        while (t <= 1f)
        {
            player.position = Vector3.Lerp(defPos, transform.position, t);
            player.localScale = Vector3.Lerp(defScale, Vector3.zero, t);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        yield return new WaitForSeconds(0.3f);
        while (t <= 1)
        {
            clear_UI.color = Color.Lerp(Color.white - Color.black, Color.white, t);
            clear_UI.rectTransform.localScale = Vector3.Lerp(Vector3.one * 1.3f, Vector3.one, t);
            t += Time.deltaTime * 2f;
            yield return null;
        }
        t = 0;
        while (t <= 1)
        {
            clear_UI_Effect.color = Color.Lerp(Color.white, Color.white - Color.black, t);
            clear_UI_Effect.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, t);
            t += Time.deltaTime * 1f;
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(SceneChange.ChangeScene("ScoreTest"));
    }
    IEnumerator Sound()
    {
        AudioSource aud = GameObject.Find("BGM").GetComponent<AudioSource>();
        stageController.PlaySE(se[0]);
        float t = 0;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            aud.volume = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        aud.volume = 0;
        stageController.PlaySE(se[1]);
        yield break;
    }


}
