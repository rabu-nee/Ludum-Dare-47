using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebuoy : MonoBehaviour {
    [SerializeField]
    private int bitesLeft = 5;
    [SerializeField]
    private float speedBoostDuration = 5f;

    [SerializeField]
    [Range(1f, 10f)]
    private float speedBoost = 2f;

    [SerializeField]
    private GameObject model;

    public Vector2 GetSpeedBoost() {
        if (bitesLeft <= 0) {
            return new Vector2(1f, 0);
        }
        else {
            bitesLeft--;
            if (bitesLeft == 0)
                model.SetActive(false);
            return new Vector2(speedBoost, speedBoostDuration);
        }
    }
}
