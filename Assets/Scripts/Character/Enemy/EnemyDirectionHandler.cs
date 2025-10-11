using UnityEngine;

public class EnemyDirectionHandler : ICharacterDirectionHandler
{
    private IAIMove enemyMovement;
    private Direction lastDirection = Direction.DownLeft;
    private Vector2 lastDirVector = Vector2.down;

    private float deadzone = 0.3f;
    private float directionChangeThreshold = 25f;

    public EnemyDirectionHandler(IAIMove enemyMovement)
    {
        this.enemyMovement = enemyMovement;
    }

    public Direction GetDirection()
    {
        Vector2 dir = enemyMovement.GetDir();

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
