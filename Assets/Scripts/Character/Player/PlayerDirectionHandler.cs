using UnityEngine;

public class PlayerDirectionHandler : MonoBehaviour, ICharacterDirectionHandler
{
    private IMovable playerMovement;
    private Direction lastDirection = Direction.Down;
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

        lastDirection = newDir;
        return newDir;
    }
}
