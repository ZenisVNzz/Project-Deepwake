using UnityEngine;

public interface IEnemyCannonNavigation
{
    Vector2 GetFireDirection();
    void ApplyRecoil();
}
