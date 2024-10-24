using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelGrid levelGrid;
    [SerializeField] private SnakeController snakeController;
    private void Start()
    {
        levelGrid = new LevelGrid(20, 20);
        snakeController.SetUp(levelGrid);
        levelGrid.SetUp(snakeController);
    }
}
