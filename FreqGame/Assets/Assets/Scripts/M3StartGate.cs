using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3StartGate : MonoBehaviour
{
    [SerializeField, Range(-20, 20)]
    int movement = -20;

    Vector3 defaultPosition;
    Vector3 positionAfterMovement;
    float time = 0;

    void Start()
    {
        defaultPosition = transform.localPosition;
        positionAfterMovement = new Vector3(defaultPosition.x, defaultPosition.y + movement, defaultPosition.z);

    }

    void Update()
    {
        time += Time.deltaTime;
        transform.localPosition = Vector3.Lerp(defaultPosition, positionAfterMovement, time / 2);
    }
}
