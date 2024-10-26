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
    public static GameManager instance;
    private GameStartingState gameStartingState;
    private static int score;
    private static int level;
    private LevelGrid levelGrid;
    [SerializeField] private SnakeController snakeController;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gameLevelPlayingTimer;
    private float gameLevelPlayingTimerMax = 10f;
    private void Awake()
    {
        instance = this;
        gameStartingState = GameStartingState.WaitingToStart;
        InitialStatic();
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
                if (gameLevelPlayingTimer < 0)
                {
                    gameStartingState = GameStartingState.GameOver;
                    OnStateChanged.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameStartingState.GameOver:
                break;
            default:
                break;
        }
    }
    public static int GetScore()
    {
        return score;
    }
    public static void AddScore()
    {
        score += 10;
    }
    public static int GetLevel()
    {
        return level;
    }
    public static void LevelUp()
    {
        level += 1;
    }
    public static void InitialStatic()
    {
        score = 0;
        level = 0;
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
    public bool IsGameOver()
    {
        return gameStartingState == GameStartingState.GameOver;
    }
}
