using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    private const string defaultBind = "{ \"bindings\":[{ \"action\":\"Player Controls/Move\",\"id\":\"a0398539-6d39-453c-b4c2-750082127243\",\"path\":\"<Keyboard>/s\",\"interactions\":\"\",\"processors\":\"\"},{ \"action\":\"Player Controls/Move\",\"id\":\"7282b1ec-b16e-49e8-bbeb-3fb0b896749a\",\"path\":\"<Keyboard>/a\",\"interactions\":\"\",\"processors\":\"\"},{ \"action\":\"Player Controls/Jump\",\"id\":\"b9fe5cae-9ff8-4d10-8441-7aa7f4e3727a\",\"path\":\"<Keyboard>/space\",\"interactions\":\"\",\"processors\":\"\"},{ \"action\":\"Player Controls/Jump\",\"id\":\"d416a4f9-2fc7-4d27-a4cc-f3316853529c\",\"path\":\"<Keyboard>/space\",\"interactions\":\"\",\"processors\":\"\"}]}";

    [SerializeField] private PlayerController player;

    bool m_paused = false;
    [SerializeField, HideInInspector] RebindControlls[] rebindControlls;

    private void Start()
    {
        LoadBinds();

        player.PlayerInput.UI.Pause.performed += _ => PauseGame();
        player.PlayerInput.PlayerControls.Pause.performed += _ => PauseGame();
    }
    private void OnValidate()
    {
        rebindControlls = GetComponentsInChildren<RebindControlls>();
    }
    public void LoadBinds()
    {
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
            player.PlayerInput.LoadBindingOverridesFromJson(rebinds);
    }

    public void SaveControlls()
    {
        string rebind = player.PlayerInput.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebind);
    }

    public void ResetControlls()
    {
        player.PlayerInput.LoadBindingOverridesFromJson(defaultBind);

        foreach (RebindControlls controll in rebindControlls)        
            controll.Reset();
    }

    public void UpdateUI()
    {
        foreach (RebindControlls controll in rebindControlls)
            controll.Reset();
    }

    public void PauseGame()
    {
        var canvases = GetComponentsInChildren<Canvas>();

        foreach (Canvas canvas in canvases)
            canvas.enabled = false;

        m_paused = !m_paused;
        canvases[1].enabled = m_paused;

        if (m_paused)
        {
            player.PlayerInput.PlayerControls.Disable();
            player.PlayerInput.UI.Enable();
        }
        else
        {
            player.PlayerInput.PlayerControls.Enable();
            player.PlayerInput.UI.Disable();
            LoadBinds();
        }
    }
    public void ShowCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }

    public void HideCanvas(Canvas canvas)
    {
        canvas.enabled = false;
    }
    public void Swap(string scene)
    {
        blu.App.GetModule<blu.SceneModule>().SwitchScene(scene, blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
    }
}
