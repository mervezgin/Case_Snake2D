using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameStartingState
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;
    public event EventHandler OnLevelChanged;
    public static GameManager instance;
    private GameStartingState gameStartingState;
    private static int level;
    private LevelGrid levelGrid;
    [SerializeField] private SnakeController snakeController;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    public float gameLevelPlayingTimer;
    public float gameLevelPlayingTimerMax = 30f;
    public bool isGamePaused = false;
    public int startWidth = 0;
    public int startHeight = 0;
    private void Awake()
    {
        instance = this;
        gameStartingState = GameStartingState.WaitingToStart;
        Score.InitialStatic();

        Score.TrySetNewHighScore(10);
    }
    private void Start()
    {
        levelGrid = new LevelGrid(20, 20);
        snakeController.SetUp(levelGrid);
        levelGrid.SetUp(snakeController);
    }
    private void Update()
    {
        switch (gameStartingState)
        {
            case GameStartingState.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0)
                {
                    gameStartingState = GameStartingState.CountDownToStart;
                    OnStateChanged.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameStartingState.CountDownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0)
                {
                    gameStartingState = GameStartingState.GamePlaying;
                    gameLevelPlayingTimer = gameLevelPlayingTimerMax;
                    OnStateChanged.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameStartingState.GamePlaying:
                gameLevelPlayingTimer -= Time.deltaTime;
                if (gameLevelPlayingTimer < 0 || SnakeController.instance.state == SnakeController.State.Dead)
                {
                    gameStartingState = GameStartingState.GameOver;
                    OnStateChanged.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameStartingState.GameOver:
                Score.TrySetNewHighScore();
                break;
            default:
                break;
        }
    }
    public bool IsGamePlaying()
    {
        return gameStartingState == GameStartingState.GamePlaying;
    }
    public bool IsCountdownToStartActive()
    {
        return gameStartingState == GameStartingState.CountDownToStart;
    }
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }
    public float GetLevelPlayingTimerNormalized()
    {
        return 1 - (gameLevelPlayingTimer / gameLevelPlayingTimerMax);
    }
    public void GamePause()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0.0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1.0f;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }
    public bool IsGameOver()
    {
        return gameStartingState == GameStartingState.GameOver;
    }
    public void LevelUp()
    {
        if (levelGrid.eatenFood == 3)
        {
            Score.AddLevel();
            levelGrid.eatenFood = 0;
            gameLevelPlayingTimer = gameLevelPlayingTimerMax;
            if (levelGrid.foodGameObject != null)
            {
                Destroy(levelGrid.foodGameObject);
            }
            int newWidth = levelGrid.width - 1;
            int newHeight = levelGrid.height - 1;
            startHeight += 1;
            startWidth += 1;

            Transform playBackground = GameObject.Find("PlayBackground").transform;
            playBackground.localScale -= new Vector3(2, 2, 0);

            levelGrid = new LevelGrid(newWidth, newHeight);
            snakeController.SetUp(levelGrid);
            levelGrid.SetUp(snakeController);
        }
    }
}
