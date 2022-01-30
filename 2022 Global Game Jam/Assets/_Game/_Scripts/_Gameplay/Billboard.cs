using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Material m_material;

    private MeshRenderer m_meshRenderer;
   
    [SerializeField] bool isSideView = true;
    [SerializeField] float rot;

    public Transform sprTransform;

    private void OnValidate()
    {
        if (sprTransform == null)
            sprTransform = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        // this is vile... but its the only way i could get the rotations to work
        rot = transform.localRotation.eulerAngles.x;
        //if (rot < -300f)
        //    rot = 0;

        isSideView = (rot <= 45);

        if(isSideView)
            sprTransform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }

    public bool isTopView
    { get { return !isSideView; } }
}