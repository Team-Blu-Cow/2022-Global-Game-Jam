using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    int colliders = 0;

    bool pressed = false;
    public bool PlatePressed => pressed;

    float m_startingY;
    float timer = 0;

    private void Awake()
    {
        m_startingY = transform.localPosition.y;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.5)
        { 
            if (colliders > 0)
            {
                if (!LeanTween.isTweening(gameObject) )                
                    gameObject.LeanMoveLocalY(m_startingY - 0.05f, 0.1f);
                
                pressed = true;
            }
            else
            {
                if (!LeanTween.isTweening(gameObject) )                
                    gameObject.LeanMoveLocalY(m_startingY + 0.05f, 0.1f);                

                pressed = false;
            }

            timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliders++;
    }

    private void OnCollisionExit(Collision collision)
    {
        colliders--;
    }
}
