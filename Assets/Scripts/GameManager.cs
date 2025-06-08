using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    CountdownTimer countdownTimer;
    CarController carController;
    // Start is called before the first frame update
    void Start()
    {
        countdownTimer = GameObject.Find("Timer").GetComponent<CountdownTimer>();
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownTimer.currentTime == 0)
        {
            carController.EnablePlayerControl(false);
        }
    }
}
