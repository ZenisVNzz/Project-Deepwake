using UnityEngine;

public interface IEnemyController
{
    public bool IsDead { get; }

    void Init();
}
