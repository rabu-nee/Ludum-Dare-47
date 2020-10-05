using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            GameManager.TriggerGameEnd(Tools.EndState.VICTORY);
        }
    }
}
