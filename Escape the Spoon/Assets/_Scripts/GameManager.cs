using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using Tools;

public class GameManager : PersistentSingleton<GameManager>
{
    public delegate void GameEnd(EndState endState);
    public static event GameEnd End;

    public static void TriggerGameEnd(EndState endState) {
        UIManager.DisplayEndGameMenu(endState);
        Debug.Log("GAME OVER");
        End?.Invoke(endState);
    }
}
