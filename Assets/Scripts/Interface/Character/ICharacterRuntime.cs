using System;
using UnityEngine;

public interface ICharacterRuntime : IAttackable
{
    float HP { get; }
    float Stamina { get; }
    float TotalHealth { get; }
    float TotalStamina { get; }
    CharacterData CharacterData { get; }

    event Action OnStatusChanged;

    void Init(CharacterData playerData, Rigidbody2D rigidbody2D, IState characterState);
}
