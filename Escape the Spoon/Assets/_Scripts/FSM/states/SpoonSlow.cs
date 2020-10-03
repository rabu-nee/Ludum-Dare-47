﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonSlow : IFSMState<SpoonController> {
    private float timer;
    private float smoothSpeed = 0.125f;
    private bool attacked;    

    public void Enter(SpoonController entity) {
        Debug.Log("Enter Slow");
        timer = Random.Range(3f, 10f);
    }

    public void Exit(SpoonController entity) {
        Debug.Log("Exit Slow");
    }

    public void Reason(SpoonController entity) {
        //if attacked and animation is over
        if (attacked) {
            if (timer > 0) {
                timer -= Time.deltaTime;
            }
            else {
                entity.RevertToIdle();
            }
        }
    }

    public void Update(SpoonController entity) {
        if (!attacked) {
            if (timer > 0) {
                timer -= Time.deltaTime;

                //follow Player
                Vector3 desiredPosition = new Vector3(entity.Player.position.x, entity.transform.position.y, entity.Player.position.z);
                Vector3 smoothedPosition = Vector3.Lerp(entity.transform.position, desiredPosition, smoothSpeed);
                entity.transform.position = smoothedPosition;

            }
            else {
                Debug.Log("attack");
                entity.SetAnimation(SpoonController.SpoonStates.SLOW);
                //get clip length and set timer
                AnimatorStateInfo clip = entity.animator.GetCurrentAnimatorStateInfo(0);
                timer = clip.length;

                attacked = true;
            }
        }
    }
}
