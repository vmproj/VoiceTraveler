using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUFOAnimation : MonoBehaviour
{

    void Start()
    {
        

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float angleOffset = -15f;
        float radius = 0.6f;
        int count = 5;
        int[] order = { 0, 3, 1, 4, 2, 0 };
        Vector3 defPos = transform.position;
        Vector3[] points = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            float rad = (360f / count * i + angleOffset) * Mathf.Deg2Rad;
            points[i] = new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0) + defPos;
        }
        int p = 0;
        while (true)
        {
            float t = 0;
            while (t <= 1)
            {
                t += Time.deltaTime * 4f;
                float st = Mathf.Sin(t * 90f * Mathf.Deg2Rad);
                transform.position = Vector3.Lerp(points[order[p % count]], points[order[(p + 1) % count]], st);
                yield return null;
            }
            p++;
            yield return new WaitForSeconds(0.08f);
        }
    }
}
