using Mirror;
using UnityEngine;

public class PlayerDirectionHandler : NetworkBehaviour, ICharacterDirectionHandler
{
    private IMovable playerMovement;
    [SyncVar] private Direction lastDirection = Direction.Down;
    private bool isForceDir = false;
    private Direction ForceDir;

    private float deadzone = 0.3f;

    private void Awake()
    {
        playerMovement = GetComponent<IMovable>();
    }

    public void EnableForceDir(Direction direction)
    {
        isForceDir = true;
        ForceDir = direction;
    }

    public void DisableForceDir()
    {
        isForceDir = false;
    }

    public Direction GetDirection()
    {
        Vector2 dir = playerMovement.GetDir();

        if (isForceDir)
        {
            switch (ForceDir)
            {
                case Direction.Up:
                    dir = Vector2.up;
                    break;
                case Direction.Down:
                    dir = Vector2.down;
                    break;
                case Direction.Right:
                    dir = Vector2.right;
                    break;
                case Direction.Left:
                    dir = Vector2.left;
                    break;
                default:
                    dir = Vector2.zero;
                    break;
            }
        }

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

        CmdSyncDirection(newDir);
        return newDir;
    }

    private void CmdSyncDirection(Direction direction)
    {
        lastDirection = direction;
    }

    public Vector2 DirectionToVector2()
    {
        return lastDirection switch
        {
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.UpLeft => (Vector2.up + Vector2.left).normalized,
            Direction.UpRight => (Vector2.up + Vector2.right).normalized,
            Direction.DownLeft => (Vector2.down + Vector2.left).normalized,
            Direction.DownRight => (Vector2.down + Vector2.right).normalized,
            _ => Vector2.zero
        };
    }
}
