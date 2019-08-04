using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
    [SerializeField]Transform[] maps;
    GateObserver gateObserver;
    [SerializeField] float rotateSpeed = 1.0f;
    StageController stageController;
    playerController playerController;

    bool isRotate = false;
    private void Start()
    {
        gateObserver = transform.parent.gameObject.GetComponent<GateObserver>();
        stageController = GameObject.Find("StageController").GetComponent<StageController>();
        playerController = GameObject.FindWithTag("Player").GetComponent<playerController>();
    }
    private void Update()
    {
        if (gateObserver.Opened && !isRotate)
        {
            StartCoroutine(RotateMap());
            isRotate = true;
        }
    }
    IEnumerator RotateMap()
    {
        var total = 0f;
        playerController.canControll = false;
        stageController.isMoving = false;

        while (total < 90f) {
            for (int i = 0; i < maps.Length; i++)
            {
                maps[i].Rotate(0, 0, -rotateSpeed);
            }
            total += rotateSpeed;
            yield return null;
        }

        stageController.isMoving = true;
        playerController.canControll = true;
        yield break;
    }
}
