using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using Unity.VisualScripting;
using UnityEngine;

public class CarRecovery : MonoBehaviour
{
    public float saveDistance = 5f;

    private Vector3 lastSavedPos;
    private Quaternion lastSavedRot;
    Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        SavePos();
    }

    void Update()
    {
        float distanceTravelled = Vector3.Distance(transform.position, lastSavedPos);
        if (distanceTravelled >= saveDistance)
        {
            SavePos();
        }

        if (Input.GetKeyDown(KeyCode.R) && carRb.velocity.magnitude < 6f && GetComponent<CarController>().playerControl)
        {
            StartCoroutine(ResetPos());
        }

        if (IsUpsideDown())
        {
            StartCoroutine(AutoReset());
        }
    }

    void SavePos()
    {
        if (GetDot() > 0.9)
        {
            lastSavedPos = transform.position + new Vector3(0,0.2f,0);
            lastSavedRot = transform.rotation;
        }
    }

    IEnumerator AutoReset()
    {
        yield return new WaitForSeconds(3);
        if (IsUpsideDown())
        {
            StartCoroutine(ResetPos());
        }
        yield return null;
    }

    IEnumerator ResetPos()
    {
        yield return new WaitForSeconds(1);
        transform.position = lastSavedPos;
        transform.rotation = lastSavedRot;

        carRb.velocity = new Vector3(0, 0, 0);
        yield return null;
    }

    bool IsUpsideDown()
    {
        return GetDot() < 0.2;
    }

    public float GetDot()
    {
        return Vector3.Dot(transform.up, Vector3.up);
    }
}
