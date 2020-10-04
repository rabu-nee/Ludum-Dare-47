using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using Tools;

public class GameManager : PersistentSingleton<GameManager>
{
    public static void TriggerGameEnd(EndState endState) {
        Debug.Log("GAME OVER");
    }
}
