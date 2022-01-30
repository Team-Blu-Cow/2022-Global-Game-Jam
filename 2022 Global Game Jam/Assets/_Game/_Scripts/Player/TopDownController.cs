using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TopDownController : PlayerStateController
{
    [SerializeField] float rotationSpeed;
    bool facingRight;

    public override void OnFixedUpdate()
    {
        rb.velocity = new Vector3(pInfo.MovementV * pInfo.groundMoveSpeed, rb.velocity.y, rb.velocity.z);
        m_animator.SetFloat("moveSpeedV", Mathf.Abs(pInfo.MovementV));

        CheckMoveDirection();
    }

    public void CheckMoveDirection()
    {
        Vector3 moveDir = new Vector3(pInfo.MovementV, 0, pInfo.MovementH );
        moveDir.Normalize();

        if(moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.up, moveDir);
            m_player.spriteTransform.rotation = Quaternion.RotateTowards(m_player.spriteTransform.rotation, toRotation, rotationSpeed);
        }

        if (pInfo.MovementH > 0.1)
            facingRight = true;
        else if (pInfo.MovementH < -0.1)
            facingRight = false;
    }

    public void SetMoveDirection(bool isFacingRight)
    {
        Vector3 moveDir;
        
        if(isFacingRight)
            moveDir = new Vector3(0, 0, -1);
        else
            moveDir = new Vector3(0, 0, 1);

        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.up, moveDir);
            m_player.spriteTransform.rotation = Quaternion.RotateTowards(m_player.spriteTransform.rotation, toRotation, rotationSpeed);
        }

        if (pInfo.MovementH > 0.1)
            facingRight = false;
        else if (pInfo.MovementH < -0.1)
            facingRight = true;
    }

    public bool GetFacingDirection()
    {
        return facingRight;
    }

    public void ResetRotation()
    {
        m_player.spriteTransform.localEulerAngles = new Vector3(0f,0f,0f);
    }
}
