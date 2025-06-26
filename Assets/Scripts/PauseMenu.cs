using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        PauseGame(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            PauseGame(!isPaused);
        }
    }

    public void PauseGame(bool pause)
    {
        pauseMenu.SetActive(pause);
        Time.timeScale = Convert.ToInt32(!pause);
        isPaused = pause;
    }
}
