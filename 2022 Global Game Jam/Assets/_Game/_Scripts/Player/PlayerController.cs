using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MasterInput input_;
    public MasterInput PlayerInput => input_;

    private Rigidbody m_rb;

    private Vector2 velocity;

    [SerializeField] private bool m_isColliding = false;
    [SerializeField] private bool m_isPulling = false;
    [SerializeField] private GameObject m_collidingBox;

    [SerializeField] bool m_grounded = true;

    [SerializeField] bool m_topDown;

    [SerializeField] float m_jumpForce;
    [SerializeField] float m_gravity;
    [SerializeField] float m_moveSpeed;
    [SerializeField] Transform m_footSensor;



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

    }

    private void Awake()
    {
        SetUpInput();

        m_rb = GetComponent<Rigidbody>();
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
           m_rb.velocity = new Vector3(-velocity.y* m_moveSpeed, m_rb.velocity.y, velocity.x* m_moveSpeed);
       else
           m_rb.velocity = new Vector3(0, m_rb.velocity.y, velocity.x*m_moveSpeed) ;

        if (!m_grounded)
            m_rb.AddForce(new Vector2(0,-m_gravity), ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        m_grounded = Physics.OverlapSphere(m_footSensor.position + new Vector3(0, 0f, 0.45f), 0.15f).Length > 1;
        m_grounded |= Physics.OverlapSphere(m_footSensor.position - new Vector3(0, 0f, 0.45f), 0.15f).Length > 1;

        if (m_isPulling)
            PullObject();
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
                m_collidingBox.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            }
            else
            {
                m_collidingBox.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }

    private void PullObject()
    {
        if (m_isColliding && m_collidingBox != null)
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
            m_isColliding = true;
            m_collidingBox = other.gameObject;
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Pullable")
        {
            m_isColliding = false;
            m_collidingBox = null;
        }
    }
}
