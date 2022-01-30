using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using blu;
using UnityEngine.SceneManagement;


public class MainMenuManager : MonoBehaviour
{
    MasterInput tempInput;
    RebindControlls[] rebindControlls;
    List<string> scenesInBuild = new List<string>();

    private void Awake()
    {
        // http://answers.unity.com/answers/1394340/view.html
        // fuck i hate unity sometimes
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            int lastSlash = scenePath.LastIndexOf("/");
            scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
        }

        tempInput = new MasterInput();
        rebindControlls = GetComponentsInChildren<RebindControlls>();      
    }

    private void Start()
    {
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
        tempInput.LoadBindingOverridesFromJson(rebinds);

        foreach (RebindControlls control in rebindControlls)
        {
            control.input = tempInput;
            control.Reset();
        }
    }

    public void LoadScene(string sceneName)
    {
        App.GetModule<SceneModule>().SwitchScene(sceneName, TransitionType.Fade, LoadingBarType.BottomRightRadial);
    }

    public void PlayLastLevel()
    {
        string lastValidScene = SceneManager.GetActiveScene().name;

        for(int i = 1; i <= 20; i++)
        {
            string sceneName = SceneNameFromIndex(i);

            if(LevelExists(sceneName))
            {
                lastValidScene = sceneName;

                if(App.GetModule<IOModule>().IsLevelCompleted(i) == false)
                {
                    App.GetModule<SceneModule>().SwitchScene(sceneName, TransitionType.Fade, LoadingBarType.BottomRightRadial);
                }
            }
        }

        App.GetModule<SceneModule>().SwitchScene(lastValidScene, TransitionType.Fade, LoadingBarType.BottomRightRadial);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }
    public void HideCanvas(Canvas canvas)
    {
        canvas.enabled = false;
    }
    public void SaveControlls()
    {
        string rebind = tempInput.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebind);
    }

    public void ResetControlls()
    {
        PlayerPrefs.SetString("rebinds", "");

        tempInput.LoadBindingOverridesFromJson("");

        foreach (RebindControlls control in rebindControlls)
        {
            control.Reset();
        }
    }

    private string SceneNameFromIndex(int index)
    {
        return "Level " + index.ToString();
    }

    private bool LevelExists(string name)
    {
        if (scenesInBuild.Contains(name))
        {
            return true;
        }

        return false;
    }


}
