using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpoonController : StatefulMonoBehaviour<SpoonController> {
    public Transform Player;

    public Animator animator;
    public enum SpoonStates { IDLE, SCOOP, SLOW, SPLASH };

    public float maxCooldown = 6f;
    public float minCoolDown = 3f;
    public bool cooldownActive;

    public Bowl bowl;

    private void Start() {
        fsm = new FSM<SpoonController>();
        fsm.Configure(this, new SpoonIdle());
    }

    public void SetAnimation(SpoonStates state) {
        animator.SetInteger("state", (int)state);
    }

    public void RevertToIdle() {
        ChangeState(new SpoonIdle());
    }

    public int ChangeAttack() {
        int randomAttack = UnityEngine.Random.Range(0, 5);
        switch ((SpoonStates)randomAttack) {
            case SpoonStates.SCOOP:
                ChangeState(new SpoonScoop());
                break;
            case SpoonStates.SLOW:
                ChangeState(new SpoonSlow());
                break;
            case SpoonStates.SPLASH:
                ChangeState(new SpoonSplash());
                break;
        }
        Debug.Log("Change attack");
        return randomAttack;
    }
}
