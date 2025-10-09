using UnityEngine;

public interface IMovable
{
    void Move();
    void Move(Vector2 input);
    Vector2 GetDir();
}
