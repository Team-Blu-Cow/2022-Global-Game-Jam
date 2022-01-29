using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SideScrollController : PlayerStateController
{
    [SerializeField] public float jumpForce;
    private bool facingRight = true;

    [SerializeField] public float CoyoteTime = 0.2f;
    private float CoyoteTimeCounter;

    [SerializeField] public float JumpBuffer = 0.2f;
    private float JumpBufferCounter;

    [SerializeField] public float IdleTime = 0.2f;
    private float IdleCounter;

    public override void OnFixedUpdate()
    {
        CheckMoveDirection();
        TryJump();
        AnimateJump();
        CheckForIdle();
    }

    private void CheckMoveDirection()
    {
        if (facingRight == true && m_player.pInfo.MovementH > 0)
            Flip();
        else if (facingRight == false && m_player.pInfo.MovementH < 0)
            Flip();
    }

    private void TryJump()
    {
        // calculate coyote time
        if (pInfo.IsGrounded)
            CoyoteTimeCounter = CoyoteTime;
        else
            CoyoteTimeCounter -= Time.deltaTime;

        // calculate jump buffer
        if (pInfo.JumpPressed)
            JumpBufferCounter = JumpBuffer;
        else
            JumpBufferCounter -= Time.deltaTime;

        // attempt to jump
        if (JumpBufferCounter > 0 && CoyoteTimeCounter > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            JumpBufferCounter = 0f;
            CoyoteTimeCounter = 0f;
        }
    }

    private void AnimateJump()
    {
        float velY = Mathf.Clamp(rb.velocity.y, -jumpForce, jumpForce);

        float lerpVal = Mathf.InverseLerp(jumpForce, -jumpForce, velY);
        float animVal = Mathf.Lerp(1, -1, lerpVal);

        m_animator.SetFloat("verticalVelocity", animVal);
    }

    private void CheckForIdle()
    {
        if (pInfo.IsGrounded && Mathf.Abs(pInfo.MovementH) < 0.1f)
            IdleCounter -= Time.deltaTime;
        else
        {
            IdleCounter = IdleTime;
            m_animator.SetBool("isIdle", false);
        }  

        if(IdleTime < 0)
            m_animator.SetBool("isIdle", true);
    }

    void Flip()
    {
        facingRight = !facingRight;
        m_player.pInfo.xscaleMult *= -1;
        Vector3 Scaler = m_player.transform.localScale;
        Scaler.x *= -1;
        m_player.transform.localScale = Scaler;
    }
}