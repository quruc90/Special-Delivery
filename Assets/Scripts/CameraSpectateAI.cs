using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpectateAI : MonoBehaviour
{
    private CamChange playerCamChange;
    private CamChange aiCamChange;
    private CamChange activeCamChange;

    void Start()
    {
        playerCamChange = GameObject.Find("Car_3").GetComponent<CamChange>();
        aiCamChange = GameObject.Find("GameManager").GetComponent<CamChange>();
        playerCamChange.enabled = true;
        aiCamChange.enabled = false;
        activeCamChange = playerCamChange;
        foreach (Camera camera in aiCamChange.cameras)
        {
            camera.enabled = false;
        }
    }

    public void TogglePlayerCameras()
    {
        if (playerCamChange.enabled)
        {
            foreach (Camera camera in playerCamChange.cameras)
            {
                camera.enabled = false;
            }
            playerCamChange.enabled = false;
            aiCamChange.enabled = true;
            activeCamChange = aiCamChange;
        }
        else
        {
            foreach (Camera camera in aiCamChange.cameras)
            {
                camera.enabled = false;
            }
            aiCamChange.enabled = false;
            playerCamChange.enabled = true;
            activeCamChange = playerCamChange;
        }
        activeCamChange.ActivateCamera();
    }
}
