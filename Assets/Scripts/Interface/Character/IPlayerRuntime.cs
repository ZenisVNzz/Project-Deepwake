using UnityEngine;

public interface IPlayerRuntime : ICharacterRuntime
{
    float Stamina { get; }
    float TotalStamina { get; }
    bool UseStamina(float amount);
    void Init(CharacterData playerData, Rigidbody2D rigidbody2D, IState PlayerState, Inventory playerInventory);
}
