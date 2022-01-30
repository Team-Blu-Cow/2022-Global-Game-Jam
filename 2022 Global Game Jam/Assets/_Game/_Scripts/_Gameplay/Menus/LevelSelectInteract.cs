using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blu;
public class LevelSelectInteract : Interactable
{
    [SerializeField] public string sceneName = "";

    protected override bool OnInteract()
    {
        if (!base.OnInteract())
            return false;

        LevelSlice slice = m_manager.GetSlice(m_manager.FindClosestSlice(gameObject));

        Debug.Log("Loading " + sceneName);
        App.GetModule<SceneModule>().SwitchScene(sceneName, TransitionType.Fade, LoadingBarType.BottomRightRadial);

        return true;
    }
}
