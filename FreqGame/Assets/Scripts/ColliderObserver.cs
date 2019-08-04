using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderObserver : MonoBehaviour
{
    string tagName = "Map";
    public string TagName
    {
        get { return tagName; }
        set { tagName = value; }
    }
    bool hit;
    public bool Hit
    {
        get { return hit; }
    }



    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == tagName && collider.gameObject.GetComponent<CollisionGimmick>() == null)
        {
            hit = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == tagName)
        {
            hit = false;
        }
    }
}
