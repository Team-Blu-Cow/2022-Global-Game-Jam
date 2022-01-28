using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool isRotating = true;

    private Material sideMaterial;
    private Material topDownMaterial;

    bool isSideView = true;

    private void Start()
    {
        sideMaterial = Resources.Load<Material>("Materials/sideMaterial");
        topDownMaterial = Resources.Load<Material>("Materials/topDownMaterial");
        GetComponent<MeshRenderer>().material = sideMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            transform.LookAt(Camera.main.transform.position, -Vector3.up);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x - 90.0f, transform.eulerAngles.y, transform.eulerAngles.z);

            if (transform.rotation.eulerAngles.x < 315.0f && transform.rotation.eulerAngles.x >= 269.0f)
            {
                if (isSideView == false)
                {
                    GetComponent<MeshRenderer>().material = sideMaterial;
                    isSideView = true;
                }
            }
            else if (transform.rotation.eulerAngles.x >= 315.0f && transform.rotation.eulerAngles.x < 359.0f)
            {
                if (isSideView == true)
                {
                    GetComponent<MeshRenderer>().material = topDownMaterial;
                    isSideView = false;
                }
            }
        }
    }
}