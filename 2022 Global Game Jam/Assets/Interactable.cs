using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    Transform m_popUp;
    [SerializeField] bool sideView;
    [SerializeField] PlayerController player;

    bool inTrigger = false;


    private void Awake()
    {
        m_popUp = GetComponentsInChildren<Transform>()[1];
    }

    private void Start()
    {
        player.PlayerInput.PlayerControls.Interact.performed += _ => OnInteract();        
    }

    protected bool OnInteract()
    {
        if (!inTrigger)
            return false;

        Debug.Log("Applied interactable");

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            OpenPopUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
            ClosePopUp();
        }
    }

    private void ClosePopUp()
    {
        Debug.Log("Left interactable");
        m_popUp.gameObject.SetActive(false);
    }
    
    private void OpenPopUp()
    {
        Debug.Log("Open interactable");

        if (sideView)
            m_popUp.localPosition = new Vector3(0.6f, m_popUp.localPosition.y , m_popUp.localPosition.z);
        else
            m_popUp.localPosition = new Vector3(-0.6f, m_popUp.localPosition.y , m_popUp.localPosition.z);

        m_popUp.gameObject.SetActive(true);
    }
}
