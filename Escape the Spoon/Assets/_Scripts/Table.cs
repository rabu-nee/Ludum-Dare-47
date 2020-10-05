using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public delegate void FruitLoopDropped();
    public static FruitLoopDropped Dropped;


    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            GameManager.TriggerGameEnd(Tools.EndState.VICTORY);
        }
        else if (collision.collider.CompareTag("FruitLoop")) {
            collision.collider.gameObject.SetActive(false);
            Dropped?.Invoke();
        }
    }
}
