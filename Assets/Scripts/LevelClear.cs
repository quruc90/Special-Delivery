using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SearchService;
using System;

public class LevelClear : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelClearText;
    [SerializeField] private Image separator;
    [SerializeField] private TextMeshProUGUI scoreCPLabel;
    [SerializeField] private TextMeshProUGUI scoreCPNum;
    [SerializeField] private TextMeshProUGUI scoreTimeLabel;
    [SerializeField] private TextMeshProUGUI scoreTimeNum;
    [SerializeField] private Image scoreSeparator;
    [SerializeField] private TextMeshProUGUI scoreTotalLabel;
    [SerializeField] private TextMeshProUGUI scoreTotalNum;
    [SerializeField] private TextMeshProUGUI continueText;
    private Score cpScore;
    private int timeBonus;
    public GameObject gameManager;

    private float separatorWidth = 0;
    // Start is called before the first frame update
    void Start()
    {
        Disable();
        gameManager = GameObject.Find("GameManager");

        cpScore = GameObject.Find("ScoreManager").GetComponent<Score>();
    }

    void Disable()
    {
        levelClearText.gameObject.SetActive(false);
        separator.gameObject.transform.localScale = new Vector3(0, 0.1f, 1);
        scoreCPLabel.gameObject.SetActive(false);
        scoreCPNum.gameObject.SetActive(false);
        scoreCPNum.text = "0";
        scoreTimeLabel.gameObject.SetActive(false);
        scoreTimeNum.gameObject.SetActive(false);
        scoreTimeNum.text = "0";
        scoreSeparator.gameObject.transform.localScale = new Vector3(0, 0.1f, 1);
        scoreTotalLabel.gameObject.SetActive(false);
        scoreTotalNum.gameObject.SetActive(false);
        scoreTotalNum.text = "0";
        continueText.gameObject.SetActive(false);
    }

    public void LevelWin()
    {
        StartCoroutine(LevelWinSequence());
        StartCoroutine(SkipSequence());
        timeBonus = Mathf.CeilToInt(GameObject.Find("Timer").GetComponent<CountdownTimer>().currentTime * 10);
    }

    IEnumerator SkipSequence()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        StopCoroutine(LevelWinSequence());

        levelClearText.gameObject.SetActive(true);
        separator.gameObject.transform.localScale = new Vector3(4, 0.1f, 1);
        scoreCPLabel.gameObject.SetActive(true);
        scoreCPNum.gameObject.SetActive(true);
        scoreTimeLabel.gameObject.SetActive(true);
        scoreTimeNum.gameObject.SetActive(true);
        scoreSeparator.gameObject.transform.localScale = new Vector3(4, 0.1f, 1);
        scoreTotalLabel.gameObject.SetActive(true);
        scoreTotalNum.gameObject.SetActive(true);
        continueText.gameObject.SetActive(true);

        levelClearText.transform.localPosition = new Vector3(0, 200, 0);
        scoreCPNum.text = cpScore.GetScore().ToString();
        scoreTimeNum.text = timeBonus.ToString();
        scoreTotalNum.text = (cpScore.GetScore() + timeBonus).ToString();

        StartCoroutine(Continue());
        yield return null;
    }

    IEnumerator LevelWinSequence()
    {
        levelClearText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);

        while (levelClearText.gameObject.transform.localPosition.y < 200)
        {
            levelClearText.gameObject.transform.Translate(new Vector3(0, 200, 0) * Time.deltaTime);
            yield return null;
        }

        while (separatorWidth < 4)
        {
            separatorWidth += 8 * Time.deltaTime;
            separator.gameObject.transform.localScale = new Vector3(separatorWidth, 0.1f, 1);
            yield return null;
        }

        scoreCPLabel.gameObject.SetActive(true);
        scoreCPNum.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        int temp = 0;
        while (int.Parse(scoreCPNum.text) < cpScore.GetScore())
        {
            temp += 6;
            scoreCPNum.text = temp.ToString();
            yield return null;
        }
        scoreCPNum.text = cpScore.GetScore().ToString();

        scoreTimeLabel.gameObject.SetActive(true);
        scoreTimeNum.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        temp = 0;
        while (int.Parse(scoreTimeNum.text) < timeBonus)
        {
            temp += 6;
            scoreTimeNum.text = temp.ToString();
            yield return null;
        }
        scoreTimeNum.text = timeBonus.ToString();

        scoreTotalLabel.gameObject.SetActive(true);
        scoreTotalNum.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        separatorWidth = 0;
        temp = 0;
        while (int.Parse(scoreCPNum.text) + int.Parse(scoreTimeNum.text) > int.Parse(scoreTotalNum.text))
        {
            temp += 4;
            scoreTotalNum.text = temp.ToString();
            separatorWidth += 4 * Time.deltaTime;
            scoreSeparator.gameObject.transform.localScale = new Vector3(separatorWidth, 0.1f, 1);
            yield return null;
        }
        scoreTotalNum.text = (cpScore.GetScore() + timeBonus).ToString();
        yield return new WaitForSeconds(1);

        StopCoroutine(SkipSequence());
        continueText.gameObject.SetActive(true);
        StartCoroutine(Continue());
        yield return null;
    }

    IEnumerator Continue()
    {
        yield return new WaitForSeconds(0.1f);
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        cpScore.UpdateScore(int.Parse(scoreTimeNum.text));
        Disable();
        gameManager.GetComponent<LoadNextLevel>().LoadNextScene();
    }
}
