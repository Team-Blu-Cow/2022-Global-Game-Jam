using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] SwitchInteractable interactbleControl;

    private void Update()
    {
        if (interactbleControl.SwitchFlipped)
        {
            if (!LeanTween.isTweening(gameObject))
                transform.LeanRotateY(90,1);
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            if (!LeanTween.isTweening(gameObject))
                transform.LeanRotateY(0, 1);
            transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        }
    }

}
