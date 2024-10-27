using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private const string GAMEOVER_TEXT = "GameOverText";
    private const string SCORE_TEXT_Number = "ScoreTextNumber";
    private const string LEVEL_TEXT_Number = "LevelTextNumber";
    private const string MAINMENU_BUTTON = "MainMenuButton";
    private const string RETRY_BUTTON = "RetryButton";
    private const string MAINMENU_SCENE = "MainMenuScene";
    private const string GAME_SCENE = "GameScene";
    private Text gameOverText;
    private Text scoreTextNumber;
    private Text levelTextNumber;
    private Button mainMenuButton;
    private Button retryButton;
    private void Awake()
    {
        gameOverText = transform.Find(GAMEOVER_TEXT).GetComponent<Text>();
        scoreTextNumber = transform.Find(SCORE_TEXT_Number).GetComponent<Text>();
        levelTextNumber = transform.Find(LEVEL_TEXT_Number).GetComponent<Text>();
        mainMenuButton = transform.Find(MAINMENU_BUTTON).GetComponent<Button>();
        retryButton = transform.Find(RETRY_BUTTON).GetComponent<Button>();

        retryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(GAME_SCENE);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(MAINMENU_SCENE);
        });
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
            scoreTextNumber.text = Score.GetScore().ToString();
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
