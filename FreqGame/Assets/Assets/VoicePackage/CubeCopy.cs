using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCopy : MonoBehaviour {

    public GameObject cube;
    public int size;
    private void Start()
    {
        for(int i = 0; i < size; i++)
        {
            Instantiate(cube, new Vector3(-10 + i*2, 0, 0)
                , Quaternion.identity);
        }
    }
}
