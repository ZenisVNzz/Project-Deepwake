using UnityEngine;

public class EnemyDirectionHandler : ICharacterDirectionHandler
{
    private IAIMove enemyMovement;
    private Direction lastDirection = Direction.DownLeft;
    private Vector2 lastDirVector = Vector2.down;
    private bool isForceDir = false;
    private Direction ForceDir;

    private float deadzone = 0.3f;
    private float directionChangeThreshold = 25f;

    private readonly bool twoWayOnly; 

    public EnemyDirectionHandler(IAIMove enemyMovement, bool twoWayOnly = false)
    {
        this.enemyMovement = enemyMovement;
        this.twoWayOnly = twoWayOnly;
        if (twoWayOnly)
        {
            lastDirection = Direction.Right; 
            lastDirVector = Vector2.right;
        }
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
        Vector2 dir = enemyMovement.GetDir();

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

        if (twoWayOnly)
        {
            if (Mathf.Abs(dir.x) < deadzone)
                return lastDirection;

            lastDirection = dir.x >= 0 ? Direction.Right : Direction.Left;
            lastDirVector = new Vector2(Mathf.Sign(dir.x), 0f);
            return lastDirection;
        }

        if (dir.sqrMagnitude < deadzone * deadzone)
            return lastDirection;

        float angle = Vector2.Angle(lastDirVector, dir);
        if (angle < directionChangeThreshold)
            return lastDirection;

        Direction newDir;

        if (dir.x >= 0 && dir.y >= 0)
            newDir = Direction.UpRight;
        else if (dir.x < 0 && dir.y >= 0)
            newDir = Direction.UpLeft;
        else if (dir.x >= 0 && dir.y < 0)
            newDir = Direction.DownRight;
        else
            newDir = Direction.DownLeft;

        lastDirection = newDir;
        lastDirVector = dir.normalized;
        return newDir;
    }
}
