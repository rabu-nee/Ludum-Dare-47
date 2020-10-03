using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonScoop : IFSMState<SpoonController> {
    private float timer;

    public void Enter(SpoonController entity) {
        Debug.Log("Enter Scoop");
        entity.SetAnimation(SpoonController.SpoonStates.SCOOP);
        AnimatorStateInfo clip = entity.animator.GetCurrentAnimatorStateInfo(0);

        timer = clip.length;
    }

    public void Exit(SpoonController entity) {
        Debug.Log("Exit Scoop");
    }

    public void Reason(SpoonController entity) {


        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            entity.RevertToIdle();
        }
    }

    public void Update(SpoonController entity) {
    }
}
