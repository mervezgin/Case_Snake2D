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
        SpawnFood();
    }
    private void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2(Random.Range(1, width - 1), Random.Range(1, height - 1));
        } while (snakeController.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }
    public bool SnakeAteFood(Vector2 snakeGridPosition)//void 
    {
        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
            return true;
        }
        else { return false; }
    }
}
