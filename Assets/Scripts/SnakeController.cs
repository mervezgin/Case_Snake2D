using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Stop
    }
    private LevelGrid levelGrid;
    private Direction gridMoveDirection;
    private Direction lastMoveDirection;
    private Vector2 gridPosition;
    private List<SnakeBodyPart> snakeBodyList;
    private List<SnakeMovePosition> snakeMovePositionList;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private int snakeBodySize;
    public void SetUp(LevelGrid levelGrid) { this.levelGrid = levelGrid; }
    private void Awake()
    {
        gridPosition = new Vector2(10, 10);
        gridMoveTimerMax = 0.2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        lastMoveDirection = gridMoveDirection;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 1;
        snakeBodyList = new List<SnakeBodyPart>();
    }
    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down && lastMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up && lastMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right && lastMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left && lastMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            gridMoveDirection = Direction.Stop; // burayla alakalı bir sıkıntı olabilir 
        }
    }
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            if (gridMoveDirection != Direction.Stop)
            {
                lastMoveDirection = gridMoveDirection;
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2 gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Up:
                    gridMoveDirectionVector = new Vector2(0, 1);
                    break;
                case Direction.Down:
                    gridMoveDirectionVector = new Vector2(0, -1);
                    break;
                case Direction.Left:
                    gridMoveDirectionVector = new Vector2(-1, 0);
                    break;
                case Direction.Right:
                    gridMoveDirectionVector = new Vector2(1, 0);
                    break;
                case Direction.Stop:
                    gridMoveDirectionVector = new Vector2(0, 0);
                    break;

            }
            gridPosition += gridMoveDirectionVector;

            bool canEat = levelGrid.SnakeAteFood(gridPosition);
            if (canEat)
            {
                snakeBodySize++;
                CreateSnakeBody();
                Debug.Log(snakeBodySize);
            }
            if (snakeMovePositionList.Count >= snakeBodySize + 1) snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            if (gridMoveDirection != Direction.Stop)
            {
                transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector));
            }
            UpdateSnakeBodyPart();
        }
    }
    private float GetAngleFromVector(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) { angle += 360; }
        return angle - 90;
    }
    public List<Vector2> GetFullSnakeGridPositionList()
    {
        List<Vector2> gridPositionList = new List<Vector2>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }
    private void CreateSnakeBody()
    {
        snakeBodyList.Add(new SnakeBodyPart(snakeBodyList.Count));
    }
    private void UpdateSnakeBodyPart()
    {
        for (int i = 0; i < snakeBodyList.Count; i++)
        {
            snakeBodyList[i].SetGridPosition(snakeMovePositionList[i].GetGridPosition());
        }
    }
    private class SnakeBodyPart
    {
        private Vector2 gridPosition;
        private Transform transform;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            //snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = bodyIndex;
            transform = snakeBodyGameObject.transform;
        }
        public void SetGridPosition(Vector2 gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
    }
    private class SnakeMovePosition
    {
        private Vector2 gridPosition;
        private Direction direction;
        public SnakeMovePosition(Vector2 gridPosition, Direction direction)
        {
            this.gridPosition = gridPosition;
            this.direction = direction;
        }
        public Vector2 GetGridPosition()
        {
            return gridPosition;
        }
    }
}
