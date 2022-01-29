using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blu;
public class LevelSelectInteract : Interactable
{
    [HideInInspector] public string sceneName = "";

    protected override bool OnInteract()
    {
        if (!base.OnInteract())
            return false;

        Debug.Log("Loading " + sceneName);
        App.GetModule<SceneModule>().SwitchScene(sceneName, TransitionType.Fade, LoadingBarType.BottomRightRadial);

        return true;
    }
}
