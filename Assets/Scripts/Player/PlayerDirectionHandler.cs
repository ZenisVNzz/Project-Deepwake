using UnityEngine;

public enum Direction
{
    Up,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
    Down,
    Left,
    Right,
}

public class PlayerDirectionHandler : ICharacterDirectionHandler
{
    private IMovable playerMovement;
    private Direction lastDirection = Direction.Down;

    private float deadzone = 0.3f;

    public PlayerDirectionHandler(IMovable playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    public Direction GetDirection()
    {
        Vector2 dir = playerMovement.GetDir();

        if (Mathf.Abs(dir.x) < deadzone) dir.x = 0;
        if (Mathf.Abs(dir.y) < deadzone) dir.y = 0;

        if (dir == Vector2.zero)
            return lastDirection;

        Direction newDir;

        if (dir.x > 0 && dir.y > 0)
            newDir = Direction.UpRight;
        else if (dir.x < 0 && dir.y > 0)
            newDir = Direction.UpLeft;
        else if (dir.x > 0 && dir.y < 0)
            newDir = Direction.DownRight;
        else if (dir.x < 0 && dir.y < 0)
            newDir = Direction.DownLeft;
        else if (dir.x > 0)
            newDir = Direction.Right;
        else if (dir.x < 0)
            newDir = Direction.Left;
        else if (dir.y > 0)
            newDir = Direction.Up;
        else
            newDir = Direction.Down;

        lastDirection = newDir;
        return newDir;
    }
}
