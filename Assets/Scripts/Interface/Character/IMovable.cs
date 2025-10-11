using UnityEngine;

public interface IMovable
{
    void Move(float moveSpeed);
    void Move(Vector2 input, float moveSpeed);
    Vector2 GetDir();
}
