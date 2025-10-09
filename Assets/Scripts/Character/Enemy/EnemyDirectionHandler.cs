using UnityEngine;

public class EnemyDirectionHandler : ICharacterDirectionHandler
{
    private IMovable enemyMovement;
    private Direction lastDirection = Direction.DownLeft;

    private float deadzone = 0.3f;

    public EnemyDirectionHandler(IMovable enemyMovement)
    {
        this.enemyMovement = enemyMovement;
    }

    public Direction GetDirection()
    {
        Vector2 dir = enemyMovement.GetDir();

        if (Mathf.Abs(dir.x) < deadzone) dir.x = 0;
        if (Mathf.Abs(dir.y) < deadzone) dir.y = 0;

        if (dir == Vector2.zero)
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
        return newDir;
    }
}
