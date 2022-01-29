using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindControlls : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private MasterInput input;
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private GameObject startRebindGO;
    [SerializeField] private GameObject waitingGO;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebind()
    {
        startRebindGO.SetActive(false);
        waitingGO.SetActive(true);

        rebindingOperation = jumpAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        int bindingIndex = jumpAction.action.GetBindingIndexForControl(jumpAction.action.controls[0]);

        displayText.text = InputControlPath.ToHumanReadableString(
            jumpAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();

        startRebindGO.SetActive(true);
        waitingGO.SetActive(false);
    }
}
