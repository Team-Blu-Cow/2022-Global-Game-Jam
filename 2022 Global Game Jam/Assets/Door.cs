using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] SwitchInteractable interactbleControl;
    [SerializeField] float animSpeed;

    private void Update()
    {
        if (interactbleControl.SwitchFlipped)
        {
            if (!LeanTween.isTweening(gameObject))
                transform.LeanRotateY(90,animSpeed);
            transform.GetComponentInParent<BoxCollider>().enabled = false;
        }
        else
        {
            if (!LeanTween.isTweening(gameObject))
                transform.LeanRotateY(0, animSpeed);
            transform.GetComponentInParent<BoxCollider>().enabled = true;
        }
    }

}
