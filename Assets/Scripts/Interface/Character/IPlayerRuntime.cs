using UnityEngine;
using System;

public interface IPlayerRuntime : ICharacterRuntime
{
    float Stamina { get; }
    float TotalStamina { get; }

    Inventory PlayerInventory { get; }
    CurrencyWallet CurrencyWallet { get; }

    event Action<float> OnStaminaChanged;
    event Action<float, float> OnExpChanged;
    event Action<int> OnLevelUp;

    bool UseStamina(float amount);
    void Init(CharacterData playerData, Rigidbody2D rigidbody2D, IState PlayerState, Inventory playerInventory);
    void GainExp(float amount);
}
