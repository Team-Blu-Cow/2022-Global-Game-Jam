using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndCanvas : MonoBehaviour
{
    //private Canvas canvas;

    [SerializeField] List<string> m_swapScenes;
  
    private void Awake()
    {
        //canvas = GetComponent<Canvas>();

        Button[] buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(Retry);
        if (m_swapScenes.Count > 1)
        {
            buttons[1].onClick.AddListener(() => Swap(m_swapScenes[0]));
            buttons[2].onClick.AddListener(() => Swap(m_swapScenes[1]));
        }

        //canvas.enabled = false;
    }

    public void Swap(string scene)
    {
        blu.App.GetModule<blu.SceneModule>().SwitchScene(scene, blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
        //canvas.enabled = false;
    }
    
    public void Retry()
    {
        blu.App.GetModule<blu.SceneModule>().SwitchScene(SceneManager.GetActiveScene().name, blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
        //canvas.enabled = false;
    }

    public void ShowCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }

    public void HideCanvas(Canvas canvas)
    {
        canvas.enabled = false;
    }
}
