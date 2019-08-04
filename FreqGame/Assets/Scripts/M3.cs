using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class M3 : MonoBehaviour
{
    GateObserver gateObserver;
    [SerializeField, Range(-20, 20)]
    int movement = -20;

    Vector3 defaultPosition;
    Vector3 positionAfterMovement;
    float time = 0;

    void Start()
    {
        defaultPosition = transform.localPosition;
        positionAfterMovement = new Vector3(defaultPosition.x, defaultPosition.y + movement, defaultPosition.z);

        gateObserver = transform.parent.gameObject.GetComponent<GateObserver>();

        this.ObserveEveryValueChanged(x => gateObserver.Opened).Where(x => x).
           Subscribe(_ => StartCoroutine(GateOpen()));
    }

    IEnumerator GateOpen()
    {
        while (time < 10f)
        {
            time += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(defaultPosition, positionAfterMovement, time / 2);
            yield return null;
        }
    }
}
