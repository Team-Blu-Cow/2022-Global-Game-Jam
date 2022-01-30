using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    bool m_paused = false;
    RebindControlls[] rebindControlls;

    private void Start()
    {
        LoadBinds();

        player.PlayerInput.UI.Pause.performed += _ => PauseGame();
        player.PlayerInput.PlayerControls.Pause.performed += _ => PauseGame();
        
        rebindControlls = GetComponentsInChildren<RebindControlls>();

        foreach (RebindControlls controll in rebindControlls)
            controll.input = player.PlayerInput;
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
        player.PlayerInput.LoadBindingOverridesFromJson("");

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
