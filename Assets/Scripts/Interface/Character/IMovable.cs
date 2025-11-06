using UnityEngine;

public interface IMovable
{
    void Move(float moveSpeed);
    void Move(Vector2 input, float moveSpeed, bool isMoveOnSlope);
    Vector2 GetDir();
}
