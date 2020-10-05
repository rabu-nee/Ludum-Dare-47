using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebuoy : MonoBehaviour {
    private int maxBites;
    private int bitesLeft;
    [SerializeField]
    private float speedBoostDuration = 3f;
    [SerializeField]
    [Range(1f, 10f)]
    private float speedBoost = 1.5f;

    [Tooltip("from smallest to biggest")]
    public GameObject[] lifebuoyStates;

    private void Start() {
        maxBites = lifebuoyStates.Length;
        bitesLeft = maxBites;
        SetLifebuoy(bitesLeft);
    }

    public delegate void DrownPlayer();
    public DrownPlayer Drown;

    public int GetBitesLeft() {
        return bitesLeft;
    }

    public void ResetBites(GameObject fruitLoop) {
        Material mat = fruitLoop.GetComponent<MeshRenderer>().material;
        foreach(GameObject go in lifebuoyStates) {
            go.GetComponent<MeshRenderer>().material = mat;
        }

        bitesLeft = maxBites;
        lifebuoyStates[maxBites-1].SetActive(true);
        SetLifebuoy(bitesLeft);
    }

    public Vector2 GetSpeedBoost() {
        if (bitesLeft <= 0) {
            return new Vector2(1f, 0);
        }
        else {
            bitesLeft--;
            SetLifebuoy(bitesLeft);
            Puppet.Sound.SoundManager.Self.PlaySound("Bug_Eating");
            //change model
            if (bitesLeft == 0) {
                Drown?.Invoke();
            }
            return new Vector2(speedBoost, speedBoostDuration);
        }
    }

    private void SetLifebuoy(int bitesLeft) {
        foreach (GameObject go in lifebuoyStates) {
            go.SetActive(false);
        }
        if (bitesLeft > 0)
            lifebuoyStates[bitesLeft-1].SetActive(true);
    }
}
