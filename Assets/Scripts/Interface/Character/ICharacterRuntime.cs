using System;
using UnityEngine;

public interface ICharacterRuntime : IAttackable
{
    float HP { get; }
    float TotalHealth { get; }
    CharacterData CharacterData { get; }

    event Action<float> OnHPChanged;

    void Init(CharacterData playerData, Rigidbody2D rigidbody2D, IState characterState);
}
