using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamChange : MonoBehaviour
{
    public Camera[] cameras;
    public Camera rearview;
    public Transform audioListener;

    public int currentCam = 0;

    void Start()
    {
        ActivateCamera();
        rearview.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameras[currentCam].enabled = false;
            rearview.enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            cameras[currentCam].enabled = true;
            rearview.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCam = (currentCam + 1) % cameras.Length;
            ActivateCamera();
        }

        Camera activeCam = rearview.enabled ? rearview : cameras[currentCam];
        audioListener.SetPositionAndRotation(activeCam.transform.position, activeCam.transform.rotation);
    }

    public void ActivateCamera()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = i == currentCam;
        }
    }
}
