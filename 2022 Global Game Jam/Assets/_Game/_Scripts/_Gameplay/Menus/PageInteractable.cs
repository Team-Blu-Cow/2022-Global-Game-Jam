using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blu;

public class PageInteractable : Interactable
{
    enum Type
    {
        Next, Last
    }

    [SerializeField] private Type type;

    protected override bool OnInteract()
    {
        if (!base.OnInteract())
            return false;

        if(type == Type.Next)
        {
            m_controller.NextPage();
        }
        else
        {
            m_controller.LastPage();
        }

        return true;
    }



}
