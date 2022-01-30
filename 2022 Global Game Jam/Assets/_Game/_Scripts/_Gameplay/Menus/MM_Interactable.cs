using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_Interactable : Interactable
{

    protected override bool OnInteract()
    {
        if (!base.OnInteract())
            return false;


        Debug.Log("Main Menu Button Pressed");

        blu.App.GetModule<blu.SceneModule>().SwitchScene("MainMenu", blu.TransitionType.Fade, blu.LoadingBarType.BottomRightRadial);

        return base.OnInteract();
    }

}
