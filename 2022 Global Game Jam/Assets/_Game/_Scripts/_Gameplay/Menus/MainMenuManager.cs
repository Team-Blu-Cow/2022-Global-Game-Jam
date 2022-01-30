using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using blu;

public class MainMenuManager : MonoBehaviour
{
    private const string defaultBind = "{ \"bindings\":[{ \"action\":\"Player Controls/Move\",\"id\":\"a0398539-6d39-453c-b4c2-750082127243\",\"path\":\"<Keyboard>/s\",\"interactions\":\"\",\"processors\":\"\"},{ \"action\":\"Player Controls/Move\",\"id\":\"7282b1ec-b16e-49e8-bbeb-3fb0b896749a\",\"path\":\"<Keyboard>/a\",\"interactions\":\"\",\"processors\":\"\"},{ \"action\":\"Player Controls/Jump\",\"id\":\"b9fe5cae-9ff8-4d10-8441-7aa7f4e3727a\",\"path\":\"<Keyboard>/space\",\"interactions\":\"\",\"processors\":\"\"},{ \"action\":\"Player Controls/Jump\",\"id\":\"d416a4f9-2fc7-4d27-a4cc-f3316853529c\",\"path\":\"<Keyboard>/space\",\"interactions\":\"\",\"processors\":\"\"}]}";

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
        PlayerPrefs.SetString("rebinds", defaultBind);

        foreach (RebindControlls control in rebindControlls)
        {
            control.Reset();
        }
    }
}
