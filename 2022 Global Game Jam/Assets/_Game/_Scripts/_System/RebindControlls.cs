using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindControlls : MonoBehaviour
{
    private TMP_Text displayText;
    private GameObject startRebindGO;
    private GameObject waitingGO;

    [SerializeField] private PlayerController player;
    [SerializeField] private string actionRebind;

    [SerializeField] private string m_composite;
    [SerializeField] private string m_compositePart;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        displayText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        startRebindGO = transform.GetChild(0).gameObject;
        waitingGO = transform.GetChild(1).gameObject;
    }

    public void StartRebind()
    {
        startRebindGO.SetActive(false);
        waitingGO.SetActive(true);

        player.PlayerInput.FindAction(actionRebind).actionMap.Disable();

        rebindingOperation = player.PlayerInput.FindAction(actionRebind).PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    public void StartCompRebind()
    {
        startRebindGO.SetActive(false);
        waitingGO.SetActive(true);

        player.PlayerInput.FindAction(actionRebind).actionMap.Disable();

        var move = player.PlayerInput.FindAction(actionRebind).ChangeBinding(m_composite);

        var comp = move.NextPartBinding(m_compositePart);

        rebindingOperation = player.PlayerInput.FindAction(actionRebind).PerformInteractiveRebinding(comp.bindingIndex)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(true))
            .Start();
    }

    private void RebindComplete(bool composite = false)
    {
        UpdateUI(composite);

        rebindingOperation.Dispose();

        player.PlayerInput.FindAction(actionRebind).actionMap.Enable();
    }

    private void UpdateUI(bool composite = false)
    {
        int bindingIndex = player.PlayerInput.FindAction(actionRebind).GetBindingIndexForControl(player.PlayerInput.FindAction(actionRebind).controls[0]);

        if (composite)
        {
            var move = player.PlayerInput.FindAction(actionRebind).ChangeBinding(m_composite);
            var comp = move.NextPartBinding(m_compositePart);
            bindingIndex = comp.bindingIndex;
        }

        displayText.text = InputControlPath.ToHumanReadableString(
            player.PlayerInput.FindAction(actionRebind).bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        startRebindGO.SetActive(true);
        waitingGO.SetActive(false);
    }

    public void Reset()
    {
        if (!string.IsNullOrEmpty(m_composite) && !string.IsNullOrEmpty(m_compositePart))
            UpdateUI(true);
        else
            UpdateUI();
    }
}
