using UnityEngine;

public class LevelGrid
{
    private SnakeController snakeController;
    private Vector2 foodGridPosition;
    private GameObject foodGameObject;
    private int height;
    private int width;
    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
    public void SetUp(SnakeController snakeController)
    {
        this.snakeController = snakeController;
    }
    private void SpawnFood()
}
