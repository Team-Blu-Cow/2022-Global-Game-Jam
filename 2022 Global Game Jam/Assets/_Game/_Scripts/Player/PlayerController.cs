using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MasterInput input_;
    public MasterInput PlayerInput => input_;

    private Rigidbody m_rb;

    private Vector2 velocity;

    private bool m_isCollidingWithPullable = false;
    private bool m_isPulling = false;
    private GameObject m_collidingBox;
    private bool m_isOnJumpPad = false;


    [SerializeField] bool m_grounded = true;

    [SerializeField] bool m_topDown;

    [SerializeField] float m_jumpForce;
    [SerializeField] float m_gravity;
    [SerializeField] float m_moveSpeed;
    [SerializeField] Transform m_footSensor;

    [SerializeField] private Animator m_animator;



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

    }

    private void Awake()
    {
        SetUpInput();

        m_rb = GetComponent<Rigidbody>();


        if (m_topDown)
            blu.App.GetModule<blu.GameStateModule>().ChangeState(blu.GameStateModule.RotationState.TOP_DOWN);
        else
            blu.App.GetModule<blu.GameStateModule>().ChangeState(blu.GameStateModule.RotationState.SIDE_ON);

    }

    public void SetUpInput()
    {
        input_ = new MasterInput();

        input_.PlayerControls.Flip.performed += _ => Flip();
        input_.PlayerControls.Jump.performed += _ => Jump();

        input_.PlayerControls.Pull.performed += _ => OnPullPerformed();
        input_.PlayerControls.Pull.canceled += _ => OnPullCanceled();

        input_.PlayerControls.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        input_.PlayerControls.Move.canceled += _ => Move(new Vector2(0, 0));

        input_.UI.Disable();
        input_.PlayerControls.Enable();
    }

    private void Jump()
    {
        if (!m_topDown && m_grounded)
        {
            m_rb.AddForce(new Vector3(0, m_jumpForce, 0), ForceMode.Impulse);
            m_grounded = false;
        }
    }

    private void Move(Vector2 in_velocity)
    {
        velocity = in_velocity;
    }

    private void Flip()
    {
        
        m_topDown = !m_topDown;
        if (m_topDown)
            blu.App.GetModule<blu.GameStateModule>().ChangeState(blu.GameStateModule.RotationState.TOP_DOWN);
        else
            blu.App.GetModule<blu.GameStateModule>().ChangeState(blu.GameStateModule.RotationState.SIDE_ON);
    }

    private void FixedUpdate()
    {
        if (m_topDown)
        {
            m_rb.velocity = new Vector3(-velocity.y * m_moveSpeed, m_rb.velocity.y, velocity.x * m_moveSpeed);
            m_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            m_rb.velocity = new Vector3(0, m_rb.velocity.y, velocity.x * m_moveSpeed);
            m_rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        if (!m_grounded)
            m_rb.AddForce(new Vector2(0,-m_gravity), ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // Layer mask was added here as overlap spheres were detecting the trigger collider on the pushable blocks
        m_grounded = Physics.OverlapSphere(m_footSensor.position + new Vector3(0, 0f, 0.45f), 0.15f, 1).Length > 1;
        m_grounded |= Physics.OverlapSphere(m_footSensor.position - new Vector3(0, 0f, 0.45f), 0.15f, 1).Length > 1;


        if (m_isPulling)
            PullObject();

        m_animator.SetBool("topDown", m_topDown);
        m_animator.SetFloat("moveSpeedZ", Mathf.Abs( velocity.x));

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_footSensor.position - new Vector3(0, 0f, 0.45f), 0.15f);
        Gizmos.DrawWireSphere(m_footSensor.position + new Vector3(0, 0f, 0.45f), 0.15f);
    }

    private void OnPullPerformed()
    {
        m_isPulling = true;
    }

    private void OnPullCanceled()
    {
        m_isPulling = false;

        if (m_collidingBox != null)
        {
            if (m_topDown)
            {
                m_collidingBox.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                m_collidingBox.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }

    private void PullObject()
    {
        if (m_isCollidingWithPullable && m_collidingBox != null)
        {
            Rigidbody boxRb = m_collidingBox.GetComponent<Rigidbody>();
            boxRb.velocity = m_rb.velocity;
            boxRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Pullable")
        {
            m_isCollidingWithPullable = true;
            m_collidingBox = other.gameObject.transform.parent.gameObject;
        }

        if (other.gameObject.tag == "JumpPad" && !m_isOnJumpPad)
        {
            if (transform.position.z > other.transform.position.z - 0.5f && transform.position.z < other.transform.position.z + 0.5f)
            {
                m_jumpForce *= other.gameObject.GetComponent<JumpPad>().GetJumpMultiplier();
                m_isOnJumpPad = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Pullable")
        {
            m_isCollidingWithPullable = false;
            m_collidingBox = null;
        }

        if (other.gameObject.tag == "JumpPad")
        {
            if (m_isOnJumpPad)
            {
                m_isOnJumpPad = false;
                m_jumpForce /= other.gameObject.GetComponent<JumpPad>().GetJumpMultiplier();
            }
        }
    }
}
