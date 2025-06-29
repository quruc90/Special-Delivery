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
        carRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    private bool IsCarStopped()
    {
        return carRB.velocity.magnitude < 0.3f;
    }

    void OnTriggerStay(Collider other)
    {
        if (IsCarStopped() && !isCoroutineStarted)
        {
            StartCoroutine(ScoreCheckpoint());
            isCoroutineStarted = true;
        }
        if (!IsCarStopped())
        {
            StopCoroutine(ScoreCheckpoint());
            isCoroutineStarted = false;
        }
    }

    IEnumerator ScoreCheckpoint()
    {
        while (IsCarStopped())
        {
            yield return new WaitForSeconds(1);
            if (IsCarStopped())
            {
                scoreScript.UpdateScore(scoreForCollecting);
                cpVis.SetActive(false);
                arrowTarget.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        
    }
}
