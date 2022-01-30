using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] SwitchInteractable switchInteractable;
    [SerializeField] PressurePlate pressureInteractable;
    [SerializeField] float animSpeed;

    bool flipped = false;

    private void Update()
    {
        if (switchInteractable)
            flipped = switchInteractable.SwitchFlipped;
        else if (pressureInteractable)
            flipped = pressureInteractable.PlatePressed;

        if (flipped)
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
