using UnityEngine;

public class EnemyDirectionHandler : MonoBehaviour, ICharacterDirectionHandler
{
    private IAIMove enemyMovement;
    private Direction lastDirection = Direction.DownLeft;
    private Vector2 lastDirVector = Vector2.down;
    private bool isForceDir = false;
    private Direction ForceDir;

    private float deadzone = 0.3f;
    private float directionChangeThreshold = 25f;

    public bool twoWayOnly; 

    public enum DirectionMode { TwoWay, FourWayDiagonal, EightWay }

    [SerializeField]
    private DirectionMode directionMode = DirectionMode.FourWayDiagonal;

    private void Awake()
    {
        this.enemyMovement = GetComponent<IAIMove>();
        if (twoWayOnly)
        {
            directionMode = DirectionMode.TwoWay;
        }
        if (directionMode == DirectionMode.TwoWay)
        {
            lastDirection = Direction.Right;
            lastDirVector = Vector2.right;
        }
        else
        {
            lastDirection = Direction.DownLeft;
            lastDirVector = Vector2.down;
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
                    if (ForceDir == Direction.UpLeft) dir = (Vector2.up + Vector2.left).normalized;
                    else if (ForceDir == Direction.UpRight) dir = (Vector2.up + Vector2.right).normalized;
                    else if (ForceDir == Direction.DownLeft) dir = (Vector2.down + Vector2.left).normalized;
                    else if (ForceDir == Direction.DownRight) dir = (Vector2.down + Vector2.right).normalized;
                    else dir = Vector2.zero;
                    break;
            }
        }

        if (directionMode == DirectionMode.TwoWay)
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

        Direction newDir = lastDirection;

        if (directionMode == DirectionMode.FourWayDiagonal)
        {
            if (dir.x >= 0 && dir.y >= 0)
                newDir = Direction.UpRight;
            else if (dir.x < 0 && dir.y >= 0)
                newDir = Direction.UpLeft;
            else if (dir.x >= 0 && dir.y < 0)
                newDir = Direction.DownRight;
            else
                newDir = Direction.DownLeft;
        }
        else
        {
            float x = Mathf.Abs(dir.x) < deadzone ? 0f : dir.x;
            float y = Mathf.Abs(dir.y) < deadzone ? 0f : dir.y;

            if (Mathf.Approximately(x, 0f) && y > 0f)
                newDir = Direction.Up;
            else if (Mathf.Approximately(x, 0f) && y < 0f)
                newDir = Direction.Down;
            else if (Mathf.Approximately(y, 0f) && x > 0f)
                newDir = Direction.Right;
            else if (Mathf.Approximately(y, 0f) && x < 0f)
                newDir = Direction.Left;
            else if (x > 0f && y > 0f)
                newDir = Direction.UpRight;
            else if (x < 0f && y > 0f)
                newDir = Direction.UpLeft;
            else if (x > 0f && y < 0f)
                newDir = Direction.DownRight;
            else
                newDir = Direction.DownLeft;
        }

        lastDirection = newDir;
        lastDirVector = dir.normalized;
        return newDir;
    }
}
