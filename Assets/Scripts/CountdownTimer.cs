using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float startTime;
    public float currentTime;
    private bool isPaused;
    void Start()
    {
        currentTime = startTime;
        isPaused = false;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (currentTime > 0f && !isPaused)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0f);

            timerText.text = currentTime.ToString("F1");

            if (currentTime <= 9.9f)
                timerText.color = Color.red;

            yield return null;
        }
    }

    public void PauseTimer(bool pause)
    {
        isPaused = pause;
    }
}
