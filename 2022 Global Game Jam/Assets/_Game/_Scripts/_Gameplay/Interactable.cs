using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    Transform m_popUp;
    [SerializeField] bool sideView;
    [SerializeField, HideInInspector] PlayerController player;
    [SerializeField] float m_radius;

    protected bool inTrigger = false;

    protected virtual void OnValidate()
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

        // todo make the editable in Editor
        if(TryGetComponent<SphereCollider>(out SphereCollider collider))
        {
            collider.isTrigger = true;
            collider.radius = m_radius;
        }
        else
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = 1;
        }        
    }

    private void Update()
    {
        if (TryGetComponent<SphereCollider>(out SphereCollider collider))        
            collider.radius = m_radius;        
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

        return true;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
            OpenPopUp();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
            ClosePopUp();
        }
    }

    protected virtual void ClosePopUp()
    {
        m_popUp.gameObject.SetActive(false);
    }

    protected virtual void OpenPopUp()
    {
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
