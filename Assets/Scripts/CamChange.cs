using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamChange : MonoBehaviour
{
    public Camera[] cameras;
    public Camera rearview;
    private int currentCam = 0;

    void Start()
    {
        ActivateCamera();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameras[currentCam].gameObject.SetActive(false);
            rearview.gameObject.SetActive(true);
        }
        
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            cameras[currentCam].gameObject.SetActive(true);
            rearview.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCam = (currentCam + 1) % cameras.Length;
            ActivateCamera();
        }
    }

    void ActivateCamera()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCam);
        }
    }
}
