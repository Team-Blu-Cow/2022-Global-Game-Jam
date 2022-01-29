using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputSystem input_;
    private Rigidbody m_rb;

    private Vector2 velocity;
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

    private void Awake()
    {
        input_ = new InputSystem();
        m_rb = GetComponent<Rigidbody>();

        Physics.gravity = new Vector3(0, -m_gravity, 0);

        input_.PlayerControls.Flip.performed += _ => Flip();
        input_.PlayerControls.Jump.performed += _ => Jump();

        input_.PlayerControls.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        input_.PlayerControls.Move.canceled += _ => Move(new Vector2(0,0));

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_footSensor.position - new Vector3(0, 0f, 0.45f), 0.15f);
        Gizmos.DrawWireSphere(m_footSensor.position + new Vector3(0, 0f, 0.45f), 0.15f);

    }
}
