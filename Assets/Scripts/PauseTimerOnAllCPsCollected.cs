using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PauseTimerOnAllCPsCollected : MonoBehaviour
{
    GameObject[] checkpoints;
    CountdownTimer countdownTimer;
    CarController carController;
    LevelClear levelClear;
    private bool debugged = false;

    void Start()
    {
        countdownTimer = GameObject.Find("Timer").GetComponent<CountdownTimer>();
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        levelClear = GameObject.Find("GameManager").GetComponent<LevelClear>();
    }

    // Update is called once per frame
    void Update()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");

        if (checkpoints.Length == 0)
        {
            countdownTimer.PauseTimer(true);
            carController.EnablePlayerControl(false);
            if (!debugged)
            {
                levelClear.LevelWin();
                debugged = true;    
            }
        }
    }
}
