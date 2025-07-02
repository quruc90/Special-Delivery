using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public int scoreNum;
    private int scoreAtLevelStart;

    void Start()
    {
        scoreNum = PlayerPrefs.GetInt("Score");
        scoreAtLevelStart = GetScore();
    }

    void Update()
    {
        Debug.Log("scoreNum: " + scoreNum.ToString());
    }

    public void ResetScoreOnLevelRestart()
    {
        SetScore(scoreAtLevelStart);
    }

    public void UpdateScore(int score)
    {
        SetScore(scoreNum + score);
    }

    public void SetScore(int score)
    {
        scoreNum = score;
        PlayerPrefs.SetInt("Score", scoreNum);
    }

    public int GetScore()
    {
        return scoreNum;
    }
}
