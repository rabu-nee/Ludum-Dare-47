using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using TMPro;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI timer;
    public GameObject
        GameEndScreen,
        ImgGameOver,
        ImgVictory;

    public void Start() {
        
    }

    public static void SetTimer(float secondsLeft) {
        Instance.timer.text = secondsLeft.ToString("0.00") + " seconds left";
    }

    public static void HideTimer() {
        Instance.timer.text = string.Empty;
    }

    public static void DisplayEndGameMenu(Tools.EndState endState) {
        Instance.GameEndScreen.SetActive(true);
        if (endState == Tools.EndState.GAME_OVER) {
            Instance.ImgGameOver.SetActive(true);
            Instance.ImgVictory.SetActive(false);
        }
        else {
            Instance.ImgGameOver.SetActive(false);
            Instance.ImgVictory.SetActive(true);
        }
    }


    public delegate void StartGame();
    public static event StartGame StartG;

    public void OnStartGameBtn() {
        Puppet.Sound.SoundManager.Self.PlaySound("Bug_Eating");
        StartG?.Invoke();
    }

    public void OnRestartBtn() {
        Puppet.Sound.SoundManager.Self.PlaySound("Bug_Eating");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnCreditsBtn(Transform box) {
        Puppet.Sound.SoundManager.Self.PlaySound("Bug_Eating");
        Debug.Log("CREDITS");
        box.DORotate(new Vector3(0f, 180f, 0f), 1f, RotateMode.LocalAxisAdd);
    }

    public void OnBackBtn(Transform box) {
        Debug.Log("CREDITS");
        box.DORotate(new Vector3(0f, -180f, 0f), 1f, RotateMode.LocalAxisAdd);
    }
}
