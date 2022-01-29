using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneSwitchTest : MonoBehaviour
{

    [SerializeField] string scene;

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            blu.App.GetModule<blu.SceneModule>().SwitchScene(scene, blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);
        }
    }
}
