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
    public MasterInput input;
    [SerializeField] private string actionRebind;

    [SerializeField] private string m_composite;
    [SerializeField] private string m_compositePart;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        displayText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        startRebindGO = transform.GetChild(0).gameObject;
        waitingGO = transform.GetChild(1).gameObject;

        if (player)
            input = player.PlayerInput;
    }

    public void StartRebind()
    {
        if (input.FindAction(actionRebind) == null)
            return;

        startRebindGO.SetActive(false);
        waitingGO.SetActive(true);

        input.FindAction(actionRebind).actionMap.Disable();

        rebindingOperation = input.FindAction(actionRebind).PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    public void StartCompRebind()
    {
        if (input.FindAction(actionRebind) == null)
            return;

        startRebindGO.SetActive(false);
        waitingGO.SetActive(true);

        input.FindAction(actionRebind).actionMap.Disable();

        var move = input.FindAction(actionRebind).ChangeBinding(m_composite);

        var comp = move.NextPartBinding(m_compositePart);

        rebindingOperation = input.FindAction(actionRebind).PerformInteractiveRebinding(comp.bindingIndex)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(true))
            .Start();
    }

    private void RebindComplete(bool composite = false)
    {
        UpdateUI(composite);

        rebindingOperation.Dispose();

        input.FindAction(actionRebind).actionMap.Enable();
    }

    private void UpdateUI(bool composite = false)
    {
        if (input.FindAction(actionRebind) == null)
            return;

        int bindingIndex = input.FindAction(actionRebind).GetBindingIndexForControl(input.FindAction(actionRebind).controls[0]);

        if (composite)
        {
            var move = input.FindAction(actionRebind).ChangeBinding(m_composite);
            var comp = move.NextPartBinding(m_compositePart);
            bindingIndex = comp.bindingIndex;
        }

        displayText.text = InputControlPath.ToHumanReadableString(
            input.FindAction(actionRebind).bindings[bindingIndex].effectivePath,
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
