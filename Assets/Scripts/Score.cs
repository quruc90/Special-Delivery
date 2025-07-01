using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject scoreUI;
    public static Score Instance;
    public int scoreNum;
    private static int scoreAtLevelStart;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        scoreAtLevelStart = Instance.GetScore();
    }

    void Update()
    {
        scoreText.text = Instance.scoreNum.ToString();
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetScoreOnLevelRestart()
    {
        scoreNum = scoreAtLevelStart;
    }

    public void UpdateScore(int score)
    {
        scoreNum += score;
    }

    public void SetScore(int score)
    {
        scoreNum = score;
    }

    public int GetScore()
    {
        return scoreNum;
    }
}
