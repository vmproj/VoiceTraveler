﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * 5f);


    }
}
