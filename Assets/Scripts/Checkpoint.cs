using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isCoroutineStarted = false;
    public Rigidbody carRB;
    public GameObject cpVis;
    public GameObject arrowTarget;
    public Score scoreScript;
    public int scoreForCollecting = 10;

    void Start()
    {
        scoreScript = GameObject.Find("Score").GetComponent<Score>();
    }

    private bool isCarStopped()
    {
        return carRB.velocity.magnitude < 0.1f;
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
                scoreScript.UpdateScore(scoreForCollecting);
                cpVis.SetActive(false);
                arrowTarget.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        
    }
}
