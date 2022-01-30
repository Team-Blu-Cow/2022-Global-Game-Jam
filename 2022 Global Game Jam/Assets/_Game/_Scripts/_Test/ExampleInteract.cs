using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleInteract : Interactable
{
    protected override bool OnInteract()
    {
        if (!base.OnInteract())
            return false;

        Debug.Log("Inherrited");

        return true;
    }
}
