using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private const string GAMEOVER_TEXT = "GameOverText";
    private const string SCORE_TEXT_Number = "ScoreTextNumber";
    private const string LEVEL_TEXT_Number = "LevelTextNumber";
    private Text gameOverText;
    private Text scoreTextNumber;
    private Text levelTextNumber;
    private void Awake()
    {
        gameOverText = transform.Find(GAMEOVER_TEXT).GetComponent<Text>();
        scoreTextNumber = transform.Find(SCORE_TEXT_Number).GetComponent<Text>();
        levelTextNumber = transform.Find(LEVEL_TEXT_Number).GetComponent<Text>();
    }
    private void Start()
    {
        GameManager.instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }
    private void Update()
    {
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.instance.IsGameOver())
        {
            scoreTextNumber.text = GameManager.GetScore().ToString();
            levelTextNumber.text = GameManager.GetLevel().ToString();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
