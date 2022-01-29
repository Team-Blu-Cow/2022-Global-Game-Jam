using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MasterInput input_;
    public MasterInput PlayerInput => input_;

    private Rigidbody m_rb;
    public Rigidbody rb => m_rb;

    [SerializeField] Transform m_footSensor;
    [SerializeField] float m_checkRadius;

    [SerializeField, HideInInspector] private Animator m_animator;
    [SerializeField, HideInInspector] private Billboard m_billboard;

    [SerializeField] private SideScrollController m_sideScrollController = new SideScrollController();
    [SerializeField] private TopDownController m_topDownController = new TopDownController();

    bool m_topDown = true;

    [SerializeField] private PlayerInfo m_info;
    public PlayerInfo pInfo => m_info;

    public Transform spriteTransform
    {
        get { return m_billboard.sprTransform; }
        set { m_billboard.sprTransform = value; }
    }

    private void OnEnable()
    {
        input_.Enable();        
    }

    private void OnDisable()
    {
        input_.Disable();
    }


    private void OnValidate()
    {
        var levelMagr = FindObjectOfType<LevelManager>();
        if(levelMagr)
        {
            levelMagr.m_playerObject = this.gameObject;
        }
        m_animator = GetComponent<Animator>();
        m_billboard = GetComponentInChildren<Billboard>();
    }

    private void Awake()
    {
        SetUpInput();

        m_rb = GetComponent<Rigidbody>();

        m_sideScrollController.Init(this, m_animator);

        m_topDownController.Init(this, m_animator);

        
    }

    public void SetUpInput()
    {
        input_ = new MasterInput();

        input_.PlayerControls.Flip.performed += _ => Flip();

        input_.UI.Disable();
        input_.PlayerControls.Enable();
    }

    private void Flip()
    {
        m_topDown = !m_topDown;
        if (m_topDown)
        {
            blu.App.GetModule<blu.GameStateModule>().ChangeState(blu.GameStateModule.RotationState.TOP_DOWN);
            m_topDownController.SetMoveDirection(m_sideScrollController.facingRight);
        }
        else
        {
            blu.App.GetModule<blu.GameStateModule>().ChangeState(blu.GameStateModule.RotationState.SIDE_ON);
            m_sideScrollController.SetFacing(m_topDownController.GetFacingDirection());
            //m_topDownController.ResetRotation();
        }
    }

    private void Update()
    {
        CollectInput();

        m_animator.SetBool("topDown", m_billboard.isTopView);
    }

    private void FixedUpdate()
    {
        CheckForGrounded();
        ApplyUniversalMovement();
       
        if (m_topDown)
            m_topDownController.OnFixedUpdate();
        else
            m_sideScrollController.OnFixedUpdate();
    }

    void CollectInput()
    {
        Vector2 vec = input_.PlayerControls.Move.ReadValue<Vector2>();

        m_info.MovementH = vec.x;
        m_info.MovementV = -vec.y;

        m_info.JumpPressedLastFrame = m_info.JumpPressedThisFrame;
        m_info.JumpPressedThisFrame = input_.PlayerControls.Jump.WasPressedThisFrame();

        m_info.JumpPressed = m_info.JumpPressedThisFrame || m_info.JumpPressedLastFrame;

        m_info.JumpDown = input_.PlayerControls.Jump.IsPressed();
        m_info.JumpReleased = input_.PlayerControls.Jump.WasReleasedThisFrame();
    }

    void CheckForGrounded()
    {
        m_info.IsGrounded = Physics.OverlapSphere(m_footSensor.position, m_checkRadius).Length > 1;
        m_info.IsGrounded |= Physics.OverlapSphere(m_footSensor.position + new Vector3(0, 0f, -0.19f), m_checkRadius).Length > 1;
        m_info.IsGrounded |= Physics.OverlapSphere(m_footSensor.position + new Vector3(0, 0f, 0.19f), m_checkRadius).Length > 1;

        m_animator.SetBool("isGrounded", m_info.IsGrounded);
    }

    void ApplyUniversalMovement()
    {
        // calculate gravity
        float fallMult = 1;
        if (m_info.IsGrounded)
            m_info.CurrentFallSpeed = 0;
        else
        {
            if (rb.velocity.y < 0)
            {
                fallMult = m_info.FallMultiplier;
            }

            m_info.CurrentFallSpeed =  m_info.CurrentFallSpeed + (m_info.FallSpeed* fallMult);

            
        }

        float velY = Mathf.Max(m_rb.velocity.y - m_info.CurrentFallSpeed, -m_info.MaxFallSpeed);

        // calculate horizontal movement speed
        m_rb.velocity = new Vector3(m_rb.velocity.x, velY, m_info.MovementH * m_info.groundMoveSpeed);

        // update animations
        m_animator.SetFloat("moveSpeedH", Mathf.Abs(m_info.MovementH));
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = (m_info.IsGrounded)? Color.green : Color.red;
        Gizmos.DrawWireSphere(m_footSensor.position, m_checkRadius);
        Gizmos.DrawWireSphere(m_footSensor.position + new Vector3(0, 0f, -0.19f), m_checkRadius);
        Gizmos.DrawWireSphere(m_footSensor.position + new Vector3(0, 0f, 0.19f), m_checkRadius);
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(m_footSensor.position , 0.15f);
    }
}

public class PlayerStateController
{
    protected PlayerController m_player;
    protected Animator m_animator;

    public PlayerInfo pInfo
    {
        get { return m_player.pInfo; }
    }

    public Rigidbody rb
    {
        get { return m_player.rb; }
    }

    public void Init(PlayerController p, Animator a)
    {
        m_player = p;
        m_animator = a;
    }

    virtual public void OnUpdate() { }
    virtual public void OnFixedUpdate() { }
}

[System.Serializable]
public class PlayerInfo
{
    public float MovementH;
    public float MovementV;

    [SerializeField] public float groundMoveSpeed;

    public float xscaleMult = 1;

    [SerializeField] public float MaxFallSpeed = 5f;
    [SerializeField] public float FallSpeed = 0.1f;
    public float CurrentFallSpeed;

    [SerializeField] public float FallMultiplier = 2.5f;
    [SerializeField] public float LowJumpMultiplier = 2f;

    public bool IsGrounded;

    [HideInInspector] public bool JumpPressedThisFrame;
    [HideInInspector] public bool JumpPressedLastFrame;
    public bool JumpPressed;

    public bool JumpDown;
    public bool JumpReleased;

    
}