using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRuntime : CharacterRuntime, IPlayerRuntime
{
    [Header("Character Bonus Stats")]
    protected float bonusStamina = 0;
    protected float bonusCriticalChance = 0;
    protected float bonusCriticalDamage = 0;

    [Header("Total Stats")]
    protected float totalStamina => characterData.Stamina + bonusStamina + (2f * Vitality);
    protected float totalCriticalChance => characterData.CriticalChance + bonusCriticalChance + (0.01f * Luck);
    protected float totalCriticalDamage => characterData.CriticalDamageMultiplier + bonusCriticalDamage;
    public float TotalStamina => totalStamina;
    public float TotalCriticalChance => totalCriticalChance;
    public float TotalCriticalDamage => totalCriticalDamage;

    [SerializeField] protected float stamina;
    public float Stamina => stamina;
    protected float _staminaRegenRate;
    protected float _staminaConsumptionMultiplier;
    private Coroutine _staminaRegenCoroutine;

    private Inventory playerInventory;
    public Inventory PlayerInventory => playerInventory;

    public void Init(CharacterData playerData, Rigidbody2D rigidbody2D, IState PlayerState, Inventory playerInventory)
    {
        base.Init(playerData, rigidbody2D, PlayerState);

        stamina = totalStamina;
        _staminaRegenRate = characterData.StaminaRegenRate;
        _staminaConsumptionMultiplier = characterData.StaminaConsumptionMultiplier;

        this.playerInventory = playerInventory;
    }

    public bool UseStamina(float amount)
    {
        float adjustedAmount = amount * _staminaConsumptionMultiplier;
        if (stamina >= adjustedAmount)
        {
            stamina -= adjustedAmount;
            _onStatusChanged?.Invoke();

            if (_staminaRegenCoroutine != null)
            {
                StopCoroutine(_staminaRegenCoroutine);
            }
            _staminaRegenCoroutine = StartCoroutine(RegenSatamina());

            return true;
        }
        return false;
    }

    private IEnumerator RegenSatamina()
    {
        yield return new WaitForSeconds(2f);
        while (stamina < totalStamina)
        {
            stamina += _staminaRegenRate * Time.deltaTime;           
            if (stamina > totalStamina)
            {
                stamina = totalStamina;
            }
            _onStatusChanged?.Invoke();
            yield return null;
        }
        _staminaRegenCoroutine = null;
    }
}
