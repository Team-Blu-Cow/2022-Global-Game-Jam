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

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {
        displayText = GetComponentsInChildren<TextMeshProUGUI>()[1];
        startRebindGO = transform.GetChild(0).gameObject;
        waitingGO = transform.GetChild(1).gameObject;
    }

    private void Start()
    {
        UpdateUI();        
    }

    public void StartRebind()
    {
        startRebindGO.SetActive(false);
        waitingGO.SetActive(true);

        player.input_.FindAction(actionRebind).actionMap.Disable();

        rebindingOperation = player.input_.FindAction(actionRebind).PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        player.input_.FindAction(actionRebind).actionMap.Disable();

        UpdateUI();

        rebindingOperation.Dispose();

        player.input_.FindAction(actionRebind).actionMap.Enable();
    }

    private void UpdateUI()
    {
        int bindingIndex = player.input_.FindAction(actionRebind).GetBindingIndexForControl(player.input_.FindAction(actionRebind).controls[0]);

        displayText.text = InputControlPath.ToHumanReadableString(
            player.input_.FindAction(actionRebind).bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        startRebindGO.SetActive(true);
        waitingGO.SetActive(false);
    }
}
