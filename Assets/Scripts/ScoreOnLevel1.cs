using UnityEngine;

public class ScoreOnLevel1 : MonoBehaviour
{
    public Score score;

    void Start()
    {
        score.SetScore(0);
    }
}
