using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    private Animator animator;
    StageController stageController;
    private float minAcc, maxAcc;
    private void Start()
    {
        animator = GetComponent<Animator>();
        stageController = GameObject.FindObjectOfType<StageController>();
        minAcc = stageController.minAcc;
        maxAcc = stageController.maxAcc;
    }
    private void Update()
    {
        animator.speed = (((stageController.GetStageAcc() - minAcc) * 1f) / (maxAcc - minAcc)) + 1.0f;
    }
    
}
