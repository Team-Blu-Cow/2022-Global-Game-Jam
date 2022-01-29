using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
            obj.AddComponent<SpriteRenderer>();
            obj.AddComponent<Billboard>();

            m_popUp = obj.transform;
            obj.transform.SetParent(transform);
        }

        SetPopUpSprite();
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

        SetPopUpSprite();

        m_popUp.gameObject.SetActive(true);
    }

    private void SetPopUpSprite()
    {
        int bindingIndex = player.PlayerInput.FindAction("Interact").GetBindingIndexForControl(player.PlayerInput.FindAction("Interact").controls[0]);

        string key = InputControlPath.ToHumanReadableString(
            player.PlayerInput.FindAction("Interact").bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        SpriteRenderer sr = m_popUp.GetComponent<SpriteRenderer>();

        if (sr.sprite.name != "UI/Light/" + key + "_Key_Light")
        {
            Sprite keySprite = Resources.Load<Sprite>("UI/Light/" + key + "_Key_Light");
            if (keySprite)
                sr.sprite = keySprite;
            else
                sr.sprite = Resources.Load<Sprite>("UI/Light/E_Key_Light");
        }
    }
}
