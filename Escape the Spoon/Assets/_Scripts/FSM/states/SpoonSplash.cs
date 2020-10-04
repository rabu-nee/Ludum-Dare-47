using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonSplash : IFSMState<SpoonController> {
    private float timer;

    public void Enter(SpoonController entity) {
        entity.SetAnimation(SpoonController.SpoonStates.SPLASH);
        AnimatorStateInfo clip = entity.animator.GetCurrentAnimatorStateInfo(0);

        timer = clip.length + Tools.Constants.TIMER_TOLERANCE;
    }

    public void Exit(SpoonController entity) {
    }

    public void Reason(SpoonController entity) {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            if (entity.IsPlayerOnSpoon())
                GameManager.TriggerGameEnd(Tools.EndState.GAME_OVER);

            entity.RevertToIdle();
        }
    }

    public void Update(SpoonController entity) {
    }
}
