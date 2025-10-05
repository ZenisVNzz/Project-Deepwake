using UnityEngine;

public interface ICharacterRuntime : IAttackable
{
    void Init(PlayerData playerData, Rigidbody2D rigidbody2D);
}
