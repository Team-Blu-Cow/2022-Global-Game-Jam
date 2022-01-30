using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using blu;


public class Interactable : MonoBehaviour
{
    Transform m_popUp;
    [SerializeField, HideInInspector] PlayerController m_player;
    [SerializeField] float m_radius;

    [SerializeField] bool m_interactSideOn;
    [SerializeField] bool m_interactTopDown;

    [SerializeField, HideInInspector] protected LevelSelectController m_controller;
    [SerializeField, HideInInspector] protected LevelManager m_manager;

    protected bool inTrigger = false;
    protected bool playerInSlice = false;

    private bool allowRun = true;

    protected virtual void OnValidate()
    {
        m_player = FindObjectOfType<PlayerController>();
        m_controller = FindObjectOfType<LevelSelectController>();
        m_manager = FindObjectOfType<LevelManager>();

        m_popUp = transform.Find("Interact Popup");
        if(m_popUp == null)
        {
            GameObject obj = new GameObject();
            obj.name = "Interact Popup";
            obj.AddComponent<SpriteRenderer>();

            m_popUp = obj.transform;
            obj.transform.SetParent(transform);
        }

        // todo make the editable in Editor
        if(TryGetComponent(out SphereCollider collider))
        {
            collider.isTrigger = true;
            if(m_radius != 0)
            {
                collider.radius = m_radius;
            }
            else
            {
                collider.radius = 1;
            }
        }
        else
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = 1;
        }        
    }

    private void OnEnable()
    {
        App.GetModule<GameStateModule>().LateOnStateChangeEvent += OnFlip;
        allowRun = true;
    }

    private void OnDisable()
    {
        App.GetModule<GameStateModule>().LateOnStateChangeEvent -= OnFlip;
        allowRun = false;
    }

    virtual protected void OnFlip(GameStateModule.RotationState state)
    {
        LevelSlice slice = m_manager.GetSlice(m_manager.FindClosestSlice(gameObject));

        if (slice.HasObject(FindObjectOfType<PlayerController>().gameObject))
        {
            playerInSlice = true;
        }
        else
        {
            playerInSlice = false;
        }

        ClosePopUp();
    }

    private void Awake()
    {
        m_popUp.gameObject.SetActive(false);
    }

    private void Start()
    {
        m_player.PlayerInput.PlayerControls.Interact.performed += _ => OnInteract(); 
    }

    private void Update()
    {
        if (inTrigger && (playerInSlice || GameStateModule.CurrentRotationState == GameStateModule.RotationState.TOP_DOWN))
        { 
            if (GameStateModule.CurrentRotationState == GameStateModule.RotationState.SIDE_ON && m_interactSideOn)
                OpenPopUp();

            if (GameStateModule.CurrentRotationState == GameStateModule.RotationState.TOP_DOWN && m_interactTopDown)
                OpenPopUp();
        }
        else
        {
            ClosePopUp();
        }
    }

    virtual protected bool OnInteract()
    {
        if (!inTrigger)
            return false;

        if (GameStateModule.CurrentRotationState == GameStateModule.RotationState.TOP_DOWN && !m_interactTopDown)
            return false;
        
        if (GameStateModule.CurrentRotationState == GameStateModule.RotationState.SIDE_ON && !m_interactSideOn)
            return false;

        if (GameStateModule.CurrentRotationState == GameStateModule.RotationState.SIDE_ON && !playerInSlice)
            return false;

        return allowRun;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }            
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }

    protected virtual void ClosePopUp()
    {
        m_popUp.gameObject.SetActive(false);
    }

    protected virtual void OpenPopUp()
    {
        if (GameStateModule.CurrentRotationState == GameStateModule.RotationState.TOP_DOWN)
            m_popUp.localPosition = new Vector3(0.6f, 0.6f , 0.6f);
        else
            m_popUp.localPosition = new Vector3(-0.6f, 0.6f , 0.6f);

        SetPopUpSprite();

        m_popUp.gameObject.SetActive(true);
    }

    private void SetPopUpSprite()
    {
        int bindingIndex = m_player.PlayerInput.FindAction("Interact").GetBindingIndexForControl(m_player.PlayerInput.FindAction("Interact").controls[0]);

        string key = InputControlPath.ToHumanReadableString(
            m_player.PlayerInput.FindAction("Interact").bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        SpriteRenderer sr = m_popUp.GetComponent<SpriteRenderer>();

        if (sr.sprite == null || sr.sprite.name != "UI/Light/" + key + "_Key_Light")
        {
            Sprite keySprite = Resources.Load<Sprite>("UI/Light/" + key + "_Key_Light");
            if (keySprite)
                sr.sprite = keySprite;
            else
                sr.sprite = Resources.Load<Sprite>("UI/Light/E_Key_Light");
        }
    }
}
