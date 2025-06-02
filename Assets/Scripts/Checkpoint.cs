using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isCoroutineStarted = false;
    public Rigidbody carRB;
    public GameObject thisCheckpoint;

    private bool isCarStopped()
    {
        return carRB.velocity.magnitude < 1;
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger entered.");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exited");
    }

    void OnTriggerStay(Collider other)
    {
        if (isCarStopped())
        {
            if (!isCoroutineStarted)
            {
                Debug.Log("Delivering...");
                StartCoroutine(ScoreCheckpoint());
                isCoroutineStarted = true;
            }
        }
        else if(isCoroutineStarted)
        {
            Debug.Log("Interrupted! Stay still to deliver...");
            isCoroutineStarted = false;
        }
    }

    IEnumerator ScoreCheckpoint()
    {
        while (isCarStopped())
        {
            yield return new WaitForSeconds(1);
            if (isCarStopped())
            {
                Debug.Log("Delivered!");
                thisCheckpoint.SetActive(false);
            }
        }
        
    }
}
