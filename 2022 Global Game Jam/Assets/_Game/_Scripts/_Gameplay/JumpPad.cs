using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float m_jumpMultiplier = 1.5f;

    public float GetJumpMultiplier()
    {
        return m_jumpMultiplier;
    }
}
