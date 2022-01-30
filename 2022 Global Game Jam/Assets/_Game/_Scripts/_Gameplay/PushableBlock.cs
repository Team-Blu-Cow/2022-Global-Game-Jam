using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    private Rigidbody rb;

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

    public void OnStateChange(blu.GameStateModule.RotationState state)
    {
        if (state == blu.GameStateModule.RotationState.SIDE_ON)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (state == blu.GameStateModule.RotationState.TOP_DOWN)
        {
            //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
}
