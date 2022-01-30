using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    private Rigidbody rb;
    public float gravityScale = 5.5f;
    public bool isGrounded = false;

    public void OnEnable()
    {
        blu.App.GetModule<blu.GameStateModule>().OnStateChangeEvent += OnStateChange;
    }

    public void OnDisable()
    {
        blu.App.GetModule<blu.GameStateModule>().OnStateChangeEvent -= OnStateChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.OverlapBox(transform.position - new Vector3(0, 0.53f, 0), new Vector3(0.4f, 0.02f, 0.4f)).Length > 1;

        if(!isGrounded)
            rb.velocity = new Vector3(rb.velocity.x, -gravityScale, rb.velocity.z);
        else
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    public void OnStateChange(blu.GameStateModule.RotationState state)
    {
        if (state == blu.GameStateModule.RotationState.SIDE_ON)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (state == blu.GameStateModule.RotationState.TOP_DOWN)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (isGrounded)? Color.green: Color.red;
        Gizmos.DrawCube(transform.position - new Vector3(0, 0.53f, 0), new Vector3(0.9f, 0.04f, 0.9f));
        Gizmos.color = Color.white;
    }
}
