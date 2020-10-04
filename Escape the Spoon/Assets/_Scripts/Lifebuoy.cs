using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebuoy : MonoBehaviour {
    public int maxBites = 4;
    private int bitesLeft;
    [SerializeField]
    private float speedBoostDuration = 3f;
    [SerializeField]
    [Range(1f, 10f)]
    private float speedBoost = 1.5f;

    [SerializeField]
    private GameObject model;

    private void Start() {
        bitesLeft = maxBites;
    }

    public delegate void DrownPlayer();
    public DrownPlayer Drown;

    public int GetBitesLeft() {
        return bitesLeft;
    }

    public void ResetBites() {
        bitesLeft = maxBites;
        model.SetActive(true);
    }

    public Vector2 GetSpeedBoost() {
        if (bitesLeft <= 0) {
            return new Vector2(1f, 0);
        }
        else {
            bitesLeft--;
            Puppet.Sound.SoundManager.Self.PlaySound("Bug_Eating");
            //change model
            if (bitesLeft == 0) {
                model.SetActive(false);
                Drown?.Invoke();
            }
            return new Vector2(speedBoost, speedBoostDuration);
        }
    }
}
