using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInteractable : Interactable
{
    bool flipped = false;

    public bool SwitchFlipped => flipped;

    protected override bool OnInteract()
    {
        if (!base.OnInteract())
            return false;

        if (flipped)
        {
            transform.GetChild(1).LeanRotateX(-40, 0.2f);
            flipped = false;
        }
        else
        {
            transform.GetChild(1).LeanRotateX(40, 0.2f);
            flipped = true;
        }

        return true;
    }
}
