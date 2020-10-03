using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonSplash : IFSMState<SpoonController> {
    private float timer;

    public void Enter(SpoonController entity) {
        Debug.Log("Enter Splash");
        entity.SetAnimation(SpoonController.SpoonStates.SPLASH);
        AnimatorStateInfo clip = entity.animator.GetCurrentAnimatorStateInfo(0);

        timer = clip.length;
    }

    public void Exit(SpoonController entity) {
        Debug.Log("Exit Splash");
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
