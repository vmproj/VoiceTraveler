using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerParticleCollision : MonoBehaviour {

    bool isCollsion;
    private void OnParticleCollision(GameObject other)
    {
        if (!isCollsion)
        {
            if (other.gameObject.tag == "Object")
            {
                var particle = other.gameObject.GetComponent<ParticleSystem>();
                var renderer = other.gameObject.GetComponent<SpriteRenderer>();
                Destroy(renderer);
                particle.Play();
                Destroy(other.gameObject, 2.0f);
                isCollsion = true;
            }
        }
    }
}
