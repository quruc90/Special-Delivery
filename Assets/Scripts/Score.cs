using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    Checkpoint[] checkpoints;
    private TextMeshProUGUI scoreText;
    private int scoreNum;

    void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        SetScore(0);
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint")
            .Select(obj => obj.GetComponent<Checkpoint>())
            .ToArray();
    }

    public void UpdateScore(int score)
    {
        scoreNum += score;
        scoreText.text = scoreNum.ToString();
    }

    void SetScore(int score)
    {
        scoreNum = score;
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return scoreNum;
    }
}
