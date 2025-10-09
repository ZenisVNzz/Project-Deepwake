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

public interface ICharacterDirectionHandler
{
    Direction GetDirection();
}
