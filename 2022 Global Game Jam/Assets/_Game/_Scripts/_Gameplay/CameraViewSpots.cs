using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewSpots : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera sideVirtualCamera;
    private Cinemachine.CinemachineVirtualCamera topVirtualCamera;
    private int primaryPriority = 10;
    private int secondaryPriority = 9;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject sideCamObject = GameObject.Find("SideCam");
        sideVirtualCamera = sideCamObject.GetComponent<Cinemachine.CinemachineVirtualCamera>();

        GameObject topDownCamObject = GameObject.Find("TopDownCam");
        topVirtualCamera = topDownCamObject.GetComponent<Cinemachine.CinemachineVirtualCamera>();

        CameraMoveToTopDownView();
    }

    public void CameraMoveToTopDownView()
    {
        sideVirtualCamera.Priority = secondaryPriority;
        topVirtualCamera.Priority = primaryPriority;
    }

    public void CameraMoveToSideView()
    {
        sideVirtualCamera.Priority = primaryPriority;
        topVirtualCamera.Priority = secondaryPriority;
    }
}
