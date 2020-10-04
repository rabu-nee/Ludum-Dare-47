using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI timer;

    public static void SetTimer(int secondsLeft) {
        Instance.timer.text = secondsLeft.ToString() + " seconds left";
    }
}
