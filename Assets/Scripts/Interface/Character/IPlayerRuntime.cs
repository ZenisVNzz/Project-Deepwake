using UnityEngine;
using System;

public interface IPlayerRuntime : ICharacterRuntime
{
    float Stamina { get; }
    float TotalStamina { get; }

    Inventory PlayerInventory { get; }

    // Raised whenever stamina value changes (consume or regen)
    event Action<float> OnStaminaChanged;

    bool UseStamina(float amount);
    void Init(CharacterData playerData, Rigidbody2D rigidbody2D, IState PlayerState, Inventory playerInventory);
}
