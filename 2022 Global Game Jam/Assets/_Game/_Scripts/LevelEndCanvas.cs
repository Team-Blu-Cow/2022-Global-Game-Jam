using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndCanvas : MonoBehaviour
{
    private Canvas canvas;
  
    private void Awake()
    {
        canvas = GetComponent<Canvas>();

        Button[] buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(Retry);
        buttons[1].onClick.AddListener(SwapA);
        buttons[2].onClick.AddListener(SwapA);

        canvas.enabled = false;
    }

    private void SwapA()
    {
        blu.App.GetModule<blu.SceneModule>().SwitchScene("Sample Scene", blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
    }
    
    private void Retry()
    {
        blu.App.GetModule<blu.SceneModule>().SwitchScene(SceneManager.GetActiveScene().name, blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
    }

    public void ShowCanvas()
    {
        canvas.enabled = true;
    }

    public void HideCanvas()
    {
        canvas.enabled = false;
    }
}
