using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnCamera : MonoBehaviour
{
    public Vector2 screenPositionNormalized = new Vector2(0.5f, 0.6f);
    public float distanceFromCamera = 3f;
    public GameObject arrow;
    public Camera currentCamera;

    void Start()
    {
        arrow = GameObject.FindGameObjectWithTag("Arrow");
    }

    void Update()
    {
        
        currentCamera = Camera.main;

        arrow.transform.SetParent(currentCamera.gameObject.transform);

        Vector3 screenPos = new Vector3(
            screenPositionNormalized.x * Screen.width,
            screenPositionNormalized.y * Screen.height,
            distanceFromCamera
        );

        arrow.transform.position = currentCamera.ScreenToWorldPoint(screenPos);
    }
}
