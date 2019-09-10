using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LandingChecker : MonoBehaviour
{
    const string fieldTag = "Field";
    public bool IsHitField
    {
        get
        {
            Collider2D[] cols = Physics2D.OverlapAreaAll(col.bounds.max, col.bounds.min);
            foreach (var c in cols)
            {
                if (c.tag == fieldTag)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool IsHitPlatform
    {
        get
        {
            Collider2D[] cols = Physics2D.OverlapAreaAll(col.bounds.max, col.bounds.min);
            foreach (var c in cols)
            {
                if (c.tag == fieldTag)
                {
                    return true;
                }
            }
            return false;
        }

    }

    public bool IsHitThisTag(string tagName)
    {
        Collider2D[] cols = Physics2D.OverlapAreaAll(col.bounds.max, col.bounds.min);
        foreach (var c in cols)
        {
            if (c.tag == tagName)
            {
                return true;
            }
        }
        return false;
    }

    Collider2D col;

    // Use this for initialization
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {

    }


}

