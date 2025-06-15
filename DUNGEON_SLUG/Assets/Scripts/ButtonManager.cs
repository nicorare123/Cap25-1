using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance == null) return;

        Button[] buttons = GetComponentsInChildren<Button>(true);
        foreach (Button btn in buttons)
        {
            switch (btn.name)
            {
                case "RetryButton":
                    btn.onClick.AddListener(() => GameManager.instance.Retry());
                    break;
                case "GoToMainButton":
                    btn.onClick.AddListener(() => GameManager.instance.GoToMain());
                    break;
                case "ExitButton":
                    btn.onClick.AddListener(() => GameManager.instance.ExitGame());
                    break;
            }
        }
    }
}
