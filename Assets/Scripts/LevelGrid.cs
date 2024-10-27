using UnityEngine;

public class LevelGrid
{
    private SnakeController snakeController;
    private Vector2 foodGridPosition;
    public GameObject foodGameObject;
    public int width;
    public int height;
    public int eatenFood = 0;
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
            foodGridPosition = new Vector2(Random.Range(GameManager.instance.startWidth + 3, width - 3), Random.Range(GameManager.instance.startHeight + 3, height - 3));
        } while (snakeController.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        CreateFood();
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }
    private void CreateFood()
    {
        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.foodSprite;
        foodGameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
    public bool SnakeAteFood(Vector2 snakeGridPosition)//void 
    {
        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            eatenFood++;
            Debug.Log(eatenFood);
            SpawnFood();
            Score.AddScore();
            GameManager.instance.LevelUp();
            return true;
        }
        else { return false; }
    }
    public bool RedZone(Vector2 gridPosition)
    {
        if (gridPosition.x == GameManager.instance.startWidth || gridPosition.x == width || gridPosition.y == GameManager.instance.startHeight || gridPosition.y == height)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
}
