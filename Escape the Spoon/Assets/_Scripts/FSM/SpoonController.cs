﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpoonController : StatefulMonoBehaviour<SpoonController> {
    public Transform
        player,
        spoonTipPos;
    public LayerMask raycastLayer;

    public Animator animator;
    public enum SpoonStates { IDLE, SCOOP, SLOW, SPLASH };

    public float maxCooldown = 6f;
    public float minCoolDown = 3f;
    public bool cooldownActive;

    public Bowl bowl;
    public float fluidDecrease = 0.1f;

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
        bowl.DecreaseFluidLevel(fluidDecrease);
        return randomAttack;
    }

    public bool IsPlayerOnSpoon() {
        RaycastHit[] hits = Physics.SphereCastAll(spoonTipPos.position, 2f, Vector3.up);
        Debug.Log("Checking if Player is on Spoon | detected collisions: " + hits.Length);
        for (int i = 0; i<hits.Length; i++) {
            if (hits[i].collider.CompareTag("Player")) {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected() {
        UnityEditor.Handles.DrawWireDisc(spoonTipPos.position, Vector3.up, 2f);
    }
}
