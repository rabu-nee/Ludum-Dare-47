﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour {
    public float
        bottomRadius,
        currentTopRadius;
    //height of the fluid
    public float fluidLevel;

    public Transform spawnPos;
    public int maxFloatingFruitLoops = 50;

    public Transform fluidPlane;
    public BoxCollider pseudoGround;

    [SerializeField]
    private GameObject[] fruitLoops;

    private float initFluidLevel;
    private float initTopRadius;

    private void Start() {
        fruitLoops = new GameObject[maxFloatingFruitLoops];
        initFluidLevel = fluidLevel;
        initTopRadius = currentTopRadius;
        fluidPlane.localScale = new Vector3(currentTopRadius*2, 0.1f, currentTopRadius*2);
        fluidPlane.transform.position = new Vector3(0, fluidLevel, 0);
        pseudoGround.size = new Vector3(currentTopRadius * 2, 0.2f, currentTopRadius * 2);
        pseudoGround.transform.position = new Vector2(0, fluidLevel - pseudoGround.size.y - 0.5f);
    }

    public void DecreaseFluidLevel(float value) {
        fluidLevel = Mathf.Clamp(fluidLevel - value, 0, initFluidLevel);
        if (fluidLevel > 0) {
            currentTopRadius = fluidLevel / initFluidLevel * initTopRadius;
            fluidPlane.localScale = new Vector3(currentTopRadius * 2, 0.1f, currentTopRadius * 2);
            fluidPlane.transform.position = new Vector3(0, fluidLevel, 0);
            pseudoGround.size = new Vector3(currentTopRadius * 2, 0.2f, currentTopRadius * 2);
            pseudoGround.transform.position = new Vector2(0, fluidLevel - pseudoGround.size.y - 0.5f);
        }
        else {
            GameManager.TriggerGameEnd(Tools.EndState.VICTORY);
        }
    }

    private void OnDrawGizmosSelected() {
        UnityEditor.Handles.DrawWireDisc(new Vector3(transform.position.x, fluidLevel, transform.position.z), Vector3.up, currentTopRadius);
    }
}
