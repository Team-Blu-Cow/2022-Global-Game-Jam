using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using blu;

public class MainMenuManager : MonoBehaviour
{
    MasterInput tempInput;
    RebindControlls[] rebindControlls;

    private void Awake()
    {
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
        int level = 0;
        bool found = true;
        while (found)
        {
            level++;
            found = App.GetModule<IOModule>().IsLevelCompleted(level);
        }

        App.GetModule<SceneModule>().SwitchScene("Level " + level, TransitionType.Fade, LoadingBarType.BottomRightRadial);
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
}
