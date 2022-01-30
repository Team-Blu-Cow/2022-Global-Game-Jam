using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEndCanvas : MonoBehaviour
{
    public void Swap(string scene)
    {
        blu.App.GetModule<blu.SceneModule>().SwitchScene(scene, blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
    }
    
    public void Retry()
    {
        blu.App.GetModule<blu.SceneModule>().SwitchScene(SceneManager.GetActiveScene().name, blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
    }

    public void NextLevel()
    {
       string sceneName =SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);

        blu.App.GetModule<blu.SceneModule>().SwitchScene(sceneName, blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
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
