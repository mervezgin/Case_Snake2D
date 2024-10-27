using System;
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
    public enum State
    {
        Alive,
        Dead
    }
    public static SnakeController instance;
    private LevelGrid levelGrid;
    private Direction gridMoveDirection;
    private Direction lastMoveDirection;
    public State state;
    private Vector2 gridPosition;
    private List<SnakeBodyPart> snakeBodyList;
    private List<SnakeMovePosition> snakeMovePositionList;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private int snakeBodySize;
    public void SetUp(LevelGrid levelGrid) { this.levelGrid = levelGrid; }

    private void Awake()
    {
        instance = this;
        gridPosition = new Vector2(10, 10);
        gridMoveTimerMax = 0.2f;
        gridMoveTimer = gridMoveTimerMax;
        state = State.Alive;
        gridMoveDirection = Direction.Right;
        lastMoveDirection = gridMoveDirection;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 1;
        snakeBodyList = new List<SnakeBodyPart>();
    }
    private void Update()
    {
        switch (state)
        {
            default: break;
            case State.Alive:
                if (GameManager.instance.IsGamePlaying())
                {

                    HandleInput();
                    HandleGridMovement();
                }
                break;
            case State.Dead:
                GameManager.instance.IsGameOver();
                break;
        }
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.GamePause();
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
            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
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
            if (!levelGrid.RedZone(gridPosition))
            {
                state = State.Dead;
                Debug.Log(levelGrid.width);
                Debug.Log(levelGrid.height);
            }

            bool canEat = levelGrid.SnakeAteFood(gridPosition);
            if (canEat)
            {
                snakeBodySize++;
                CreateSnakeBody();
            }
            if (snakeMovePositionList.Count >= snakeBodySize + 1) snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);

            UpdateSnakeBodyPart();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyList)
            {
                Vector2 snakeBodyGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyGridPosition)
                {
                    state = State.Dead;
                }
            }
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            if (gridMoveDirection != Direction.Stop)
            {
                transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector));
            }
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
            snakeBodyList[i].SetSnakeBodyMovePosition(snakeMovePositionList[i]);
        }
    }
    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            transform = snakeBodyGameObject.transform;
        }
        public void SetSnakeBodyMovePosition(SnakeMovePosition snakeMovePosition)
        {
            float bodyAngle;
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            bodyAngle = 0.0f;
                            break;
                        case Direction.Left:
                            bodyAngle = 0 + 45;
                            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y);
                            break;
                        case Direction.Right:
                            bodyAngle = 0 - 45;
                            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y + 0.2f);
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            bodyAngle = 180.0f;
                            break;
                        case Direction.Left:
                            bodyAngle = 180 - 45;
                            transform.position = new Vector3(transform.position.x, transform.position.y);
                            break;
                        case Direction.Right:
                            bodyAngle = 180 + 45;
                            transform.position = new Vector3(transform.position.x, transform.position.y);
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            bodyAngle = -90.0f;
                            break;
                        case Direction.Down:
                            bodyAngle = -45;
                            transform.position = new Vector3(transform.position.x, transform.position.y);
                            break;
                        case Direction.Up:
                            bodyAngle = 45;
                            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y);
                            break;
                    }
                    break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            bodyAngle = 90.0f;
                            break;
                        case Direction.Down:
                            bodyAngle = 45;
                            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y + 0.2f);
                            break;
                        case Direction.Up:
                            bodyAngle = -45;
                            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y - 0.2f);
                            break;
                    }
                    break;
            }
            transform.eulerAngles = new Vector3(0, 0, bodyAngle);
        }
        public Vector2 GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }
    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2 gridPosition;
        private Direction direction;
        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2 gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }
        public Vector2 GetGridPosition()
        {
            return gridPosition;
        }
        public Direction GetDirection()
        {
            return direction;
        }
        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
        }
    }
}
