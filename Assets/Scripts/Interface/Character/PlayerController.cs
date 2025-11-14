using UnityEngine;

public interface IPlayerController
{
    public bool IsDead { get; }

    public void MoveOnSlope(bool moveOnSlope);

    void Init();
}
