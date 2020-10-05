using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI timer;
    public GameObject
        GameEndScreen;

    public void Start() {
        
    }

    public static void SetTimer(float secondsLeft) {
        Instance.timer.text = secondsLeft.ToString("0.00") + " seconds left";
    }

    public static void DisplayEndGameMenu(Tools.EndState endState) {
        Instance.timer.text = endState.ToString();
        Instance.GameEndScreen.SetActive(true);
    }


    public delegate void StartGame();
    public static event StartGame StartG;

    public void OnStartGameBtn() {
        StartG?.Invoke();
    }

    public void OnRestartBtn() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnCreditsBtn() {
        Debug.Log("CREDITS");
    }
}
