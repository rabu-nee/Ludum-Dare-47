﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonIdle : IFSMState<SpoonController> {
    private float timer;

    public void Enter(SpoonController entity) {
        entity.SetAnimation(SpoonController.SpoonStates.IDLE);
        timer = Random.Range(3f, 6f);
    }

    public void Exit(SpoonController entity) {
    }

    public void Reason(SpoonController entity) {
        if (timer > 0) {
            timer -= Time.deltaTime;
        }
        else {
            entity.ChangeAttack();
        }
    }

    public void Update(SpoonController entity) {
    }
}
