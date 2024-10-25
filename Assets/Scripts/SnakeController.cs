using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private LevelGrid levelGrid;
    private Vector2 gridMoveDirection;
    private Vector2 lastMoveDirection;
    private Vector2 gridPosition;
    private List<Transform> snakeBodyList;
    private float gridMoveTimer;
    private float gridMoveTimerMax;

    private void Awake()
    {
        gridPosition = new Vector2(10, 10);
        gridMoveTimerMax = 0.5f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2(1, 0);
        lastMoveDirection = gridMoveDirection;
    }
    private void Start()
    {
        snakeBodyList = new List<Transform>();
        snakeBodyList.Add(this.transform);
    }
    public void SetUp(LevelGrid levelGrid) { this.levelGrid = levelGrid; }
    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection.y != -1 && lastMoveDirection.y != -1)
            {
                gridMoveDirection = Vector2.up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection.y != +1 && lastMoveDirection.y != 1)
            {
                gridMoveDirection = Vector2.down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection.x != +1 && lastMoveDirection.x != +1)
            {
                gridMoveDirection = Vector2.left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection.x != -1 && lastMoveDirection.x != -1)
            {
                gridMoveDirection = Vector2.right;
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            gridMoveDirection = Vector2.zero;
        }
    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            if (gridMoveDirection != Vector2.zero)
            {
                lastMoveDirection = gridMoveDirection;
            }
            gridPosition += gridMoveDirection;

            for (int i = snakeBodyList.Count - 1; i > 0; i--)
            {
                snakeBodyList[i].transform.position = snakeBodyList[i - 1].transform.position;
            }

            gridMoveTimer -= gridMoveTimerMax;

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            if (gridMoveDirection != Vector2.zero)
            {
                transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));
            }
            levelGrid.SnakeAteFood(gridPosition);
        }
    }
    private float GetAngleFromVector(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) { angle += 360; }
        return angle - 90;
    }
    public Vector2 GetGridPosition()
    {
        return gridPosition;
    }
    public void SnakeBodyGrow()
    {
        Transform snakeBody = Instantiate(GameAssets.instance.snakeBodyGameObject);
        snakeBody.position = snakeBodyList[snakeBodyList.Count - 1].position;
        snakeBodyList.Add(snakeBody);
    }
}
