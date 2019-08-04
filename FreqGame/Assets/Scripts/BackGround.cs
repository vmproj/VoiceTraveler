using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{

    Transform map;
    Vector3 previousPos;
    float mapMovement;


    void Start()
    {

        map = GameObject.Find("map").transform;
        previousPos = map.position;
    }

    void Update()
    {

        mapMovement = (map.position.x - previousPos.x);

        previousPos = map.position;
    }
}
