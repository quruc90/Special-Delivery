using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    private int currentLevel;
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadNextScene()
    {
        if (currentLevel + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentLevel + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
