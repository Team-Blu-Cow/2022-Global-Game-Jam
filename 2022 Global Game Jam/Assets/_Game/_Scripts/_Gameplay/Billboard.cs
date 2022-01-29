using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool isRotating = true;

    private Material m_material;

    private MeshRenderer m_meshRenderer;
   
    [SerializeField] bool isSideView = true;
    [SerializeField] float rot;

    private void Start()
    {
        var obj = GetComponentInChildren<MeshRenderer>().gameObject;
        m_meshRenderer = obj.GetComponent<MeshRenderer>();
        m_meshRenderer.material = m_material;

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        // this is vile... but its the only way i could get the rotations to work
        rot = transform.localRotation.eulerAngles.y - 360;
        if (rot < -300f)
            rot = 0;

        isSideView = (rot <= -45);
        
        // TODO @adam: add 2d animation on 3d object code here

        /*if (isRotating)
        {
            transform.LookAt(Camera.main.transform.position, -Vector3.up);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x - 90.0f, transform.eulerAngles.y, transform.eulerAngles.z);

            if (transform.eulerAngles.x < 0.0f)
            {
                transform.localRotation = Quaternion.Euler(-180.0f, transform.eulerAngles.y, transform.eulerAngles.z);
            }

            ConsoleProDebug.Watch("Billboard rotation", transform.eulerAngles.x.ToString());


            if (transform.rotation.eulerAngles.x < 315.0f && transform.rotation.eulerAngles.x >= 270.0f)
            {
                if (isSideView == false)
                {
                    m_meshRenderer.material = sideMaterial;
                    isSideView = true;
                }
            }
            else if (transform.rotation.eulerAngles.x >= 315.0f && transform.rotation.eulerAngles.x < 359.0f)
            {
                if (isSideView == true)
                {
                    m_meshRenderer.material = topDownMaterial;
                    isSideView = false;
                }
            }
        }*/
    }
}