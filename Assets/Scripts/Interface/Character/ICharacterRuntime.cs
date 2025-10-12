using UnityEngine;

public interface ICharacterRuntime : IAttackable
{
    float HP { get; }

    CharacterData CharacterData { get; }

    void Init(CharacterData playerData, Rigidbody2D rigidbody2D, IState characterState);
}
