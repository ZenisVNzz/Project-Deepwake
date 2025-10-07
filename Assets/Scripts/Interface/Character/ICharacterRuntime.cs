using UnityEngine;

public interface ICharacterRuntime : IAttackable
{
    float HP { get; }

    CharacterData PlayerData { get; }

    void Init(CharacterData playerData, Rigidbody2D rigidbody2D);
}
