using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    private const string MAINMENU_BUTTON = "MainMenuButton";
    private const string RESUME_BUTTON = "ResumeButton";
    private const string MAINMENU_SCENE = "MainMenuScene";
    private Button mainMenuButton;
    private Button resumeButton;

    private void Awake()
    {
        mainMenuButton = transform.Find(MAINMENU_BUTTON).GetComponent<Button>();
        resumeButton = transform.Find(RESUME_BUTTON).GetComponent<Button>();

        resumeButton.onClick.AddListener(() =>
        {
            GameManager.instance.GamePause();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(MAINMENU_SCENE);
        });
    }
    private void Start()
    {
        GameManager.instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.instance.OnGameUnPaused += GameManager_OnGameUnPaused;
        Hide();
    }

    private void GameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        Show();
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
