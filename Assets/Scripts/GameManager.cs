using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static int score;
    private LevelGrid levelGrid;
    [SerializeField] private SnakeController snakeController;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        levelGrid = new LevelGrid(20, 20);
        snakeController.SetUp(levelGrid);
        levelGrid.SetUp(snakeController);
    }
    public static int GetScore()
    {
        return score;
    }
    public static void AddScore()
    {
        score += 10;
    }
}
