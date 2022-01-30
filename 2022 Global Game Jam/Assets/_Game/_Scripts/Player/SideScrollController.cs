using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SideScrollController : PlayerStateController
{
    [SerializeField] public float jumpForce;
    public bool facingRight = true;

    [SerializeField] public float CoyoteTime = 0.2f;
    private float CoyoteTimeCounter;

    [SerializeField] public float JumpBuffer = 0.2f;
    private float JumpBufferCounter;

    public override void OnFixedUpdate()
    {
        CheckMoveDirection();
        TryJump();
        AnimateJump();
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

    void Flip()
    {
        facingRight = !facingRight;
        m_player.pInfo.xscaleMult *= -1;
        Vector3 Scaler = m_player.transform.localScale;
        Scaler.x *= -1;
        m_player.transform.localScale = Scaler;
    }

    public void SetFacing(bool isFacingRight)
    {
        facingRight = isFacingRight;
        m_player.pInfo.xscaleMult = (isFacingRight) ? 1 : -1;
        Vector3 Scaler = m_player.transform.localScale;
        Scaler.x = Mathf.Abs(m_player.transform.localScale.x) * m_player.pInfo.xscaleMult;
        m_player.transform.localScale = Scaler;
    }

    public override void OnUpdate()
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
    }
}