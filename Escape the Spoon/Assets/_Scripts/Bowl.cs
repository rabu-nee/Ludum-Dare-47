﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour {
    public float
        bottomRadius,
        currentTopRadius;
    //height of the fluid
    public float fluidLevel;

    public float fruitLoopsForce = 8f;
    public Transform spawnPos;
    public int maxFloatingFruitLoops = 50;

    public float fluidGroundOffset = 0.3f;
    public Transform fluidPlane;
    public BoxCollider pseudoGround;

    [SerializeField]
    private Transform fruitLoopsContainer;
    [SerializeField]
    private GameObject[] fruitLoops;

    private float initFluidLevel;
    private float initTopRadius;

    private void Start() {
        fruitLoops = new GameObject[maxFloatingFruitLoops];
        initFluidLevel = fluidLevel;
        initTopRadius = currentTopRadius;

        DecreaseFluidLevel(0f);
    }

    private void GenerateFruitLoops() {
        StartCoroutine(GenerateFruitLoopsRoutine());
    }

    private IEnumerator GenerateFruitLoopsRoutine() {
        for (int i = 0; i < maxFloatingFruitLoops; i++) {
            GameObject go = ObjectPooler.SpawnFromPool("FRUITLOOP", spawnPos.position, Quaternion.identity);
            go.GetComponent<Rigidbody>().AddForce(spawnPos.forward * -fruitLoopsForce, ForceMode.VelocityChange);
            fruitLoops[i] = go;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void OnEnable() {
        UIManager.StartG += GenerateFruitLoops;
        SpoonController.Eaten += CheckActiveFruitLoops;
        Table.Dropped += CheckActiveFruitLoops;
    }

    private void OnDisable() {
        UIManager.StartG -= GenerateFruitLoops;
        SpoonController.Eaten -= CheckActiveFruitLoops;
        Table.Dropped -= CheckActiveFruitLoops;
    }

    private void CheckActiveFruitLoops() {
        Debug.Log("Checking activity state");
        for (int i = 0; i < fruitLoops.Length; i++) {
            Debug.Log(fruitLoops[i] + " is " + fruitLoops[i].activeSelf);

            if (fruitLoops[i].activeSelf) {
                Debug.Log(fruitLoops[i] + " is " + fruitLoops[i].activeSelf);
                return;
            }
        }

        //none were active, no fruit loops left
        GameManager.TriggerGameEnd(Tools.EndState.VICTORY);
    }

    public void DecreaseFluidLevel(float value) {
        fluidLevel = Mathf.Clamp(fluidLevel - value, 0, initFluidLevel);
        if (fluidLevel > 0) {
            currentTopRadius = fluidLevel / initFluidLevel * initTopRadius;
            fluidPlane.localScale = new Vector3(1 / initTopRadius * currentTopRadius, 1f, 1 / initTopRadius * currentTopRadius);
            fluidPlane.transform.position = new Vector3(0, fluidLevel, 0);
            pseudoGround.size = new Vector3((currentTopRadius + 0.3f) * 2, 1f, (currentTopRadius + 0.3f) * 2);
            pseudoGround.transform.position = new Vector2(0, fluidLevel - pseudoGround.size.y - fluidGroundOffset);
        }
        else {
            GameManager.TriggerGameEnd(Tools.EndState.VICTORY);
        }
    }

    public float GetInitTopRadius() {
        return initTopRadius;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        UnityEditor.Handles.DrawWireDisc(new Vector3(transform.position.x, fluidLevel, transform.position.z), Vector3.up, currentTopRadius);
    }
#endif
}
