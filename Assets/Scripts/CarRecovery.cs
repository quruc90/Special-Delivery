using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using UnityEngine;

public class CarRecovery : MonoBehaviour
{
    public float saveDistance = 5f;

    private Vector3 lastSavedPos;
    private Quaternion lastSavedRot;

    // Start is called before the first frame update
    void Start()
    {
        SavePos();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceTravelled = Vector3.Distance(transform.position, lastSavedPos);
        if (distanceTravelled >= saveDistance)
        {
            SavePos();
        }

        if (Input.GetKeyDown(KeyCode.R) && IsUpsideDown())
        {
            ResetPos();
        }
    }

    void SavePos()
    {
        if (GetDot() > 0.2)
        {
            lastSavedPos = transform.position;
            lastSavedRot = transform.rotation;
        }
    }

    void ResetPos()
    {
        transform.position = lastSavedPos;
        transform.rotation = lastSavedRot;

        CarController cc = GetComponent<CarController>();
        cc.carRb.velocity = new Vector3(0, 0, 0);
    }

    bool IsUpsideDown()
    {

        return GetDot() < 0;
    }

    public float GetDot()
    {
        return Vector3.Dot(transform.up, Vector3.up);
    }
}
