using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{


    Transform m_popUp;
    [SerializeField] bool sideView;
    [SerializeField, HideInInspector] PlayerController player;

    bool inTrigger = false;

    private void OnValidate()
    {
        player = FindObjectOfType<PlayerController>();

        m_popUp = transform.Find("Interact Popup");
        if(m_popUp == null)
        {
            GameObject obj = new GameObject();
            obj.name = "Interact Popup";
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            obj.AddComponent<Billboard>();

            sr.sprite = Resources.Load<Sprite>("UI/Light/E_Key_Light");

            m_popUp = obj.transform;
            obj.transform.SetParent(transform);
        }
    }


    private void Awake()
    {
        m_popUp.gameObject.SetActive(false);
    }

    private void Start()
    {
        player.PlayerInput.PlayerControls.Interact.performed += _ => OnInteract();        
    }

    virtual protected bool OnInteract()
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
            m_popUp.localPosition = new Vector3(0.6f, 0.6f , 0.6f);
        else
            m_popUp.localPosition = new Vector3(-0.6f, 0.6f, 0.6f);

        m_popUp.gameObject.SetActive(true);
    }
}
