using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private int score;

    void Start()
    {
        scoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        score = PlayerPrefs.GetInt("Score");
        scoreText.text = score.ToString();
    }
}
