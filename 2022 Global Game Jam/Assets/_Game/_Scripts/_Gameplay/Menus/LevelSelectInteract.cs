using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blu;
public class LevelSelectInteract : Interactable
{
    [HideInInspector] public string sceneName = "";
    LevelSelectController m_controller;
    LevelManager m_manager;

    bool inSlice = false;

    private void OnEnable()
    {
        App.GetModule<GameStateModule>().LateOnStateChangeEvent += OnFlip;
    }

    private void OnDisable()
    {
        App.GetModule<GameStateModule>().LateOnStateChangeEvent -= OnFlip;
    }

    private void OnFlip(GameStateModule.RotationState state)
    {
        LevelSlice slice = m_manager.GetSlice(m_manager.FindClosestSlice(gameObject));

        if(slice.HasObject(FindObjectOfType<PlayerController>().gameObject))
        {
            inSlice = true;
        }
        else
        {
            inSlice = false;
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        m_controller = FindObjectOfType<LevelSelectController>();
        m_manager = FindObjectOfType<LevelManager>();
    }

    protected override bool OnInteract()
    {
        if (!base.OnInteract())
            return false;

        if (GameStateModule.CurrentRotationState == GameStateModule.RotationState.TOP_DOWN)
            return false;

        if (!inSlice)
            return false;

        Debug.Log("Loading " + sceneName);
        App.GetModule<SceneModule>().SwitchScene(sceneName, TransitionType.Fade, LoadingBarType.BottomRightRadial);

        return true;
    }

    private void Update()
    {
        if (inTrigger && inSlice && GameStateModule.CurrentRotationState == GameStateModule.RotationState.SIDE_ON)
            OpenPopUp();
        else
            ClosePopUp();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }


}
