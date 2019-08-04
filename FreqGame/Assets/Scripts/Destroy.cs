using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{

    private Rigidbody2D rd2D;
    private float thrust = 50.0f;
    private void Start()
    {
        rd2D = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
            rd2D.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * thrust, ForceMode2D.Impulse);
    }
    private void FixedUpdate()
    {

    }
}
