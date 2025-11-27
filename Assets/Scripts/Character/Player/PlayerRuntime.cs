using Mirror;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class PlayerRuntime : CharacterRuntime, IPlayerRuntime
{
    [Header("Experience System")]
    [SerializeField][SyncVar] protected float currentExp = 0;
    [SerializeField][SyncVar] protected float expToNextLevel = 100;
    public float CurrentExp => currentExp;
    public float ExpToNextLevel => expToNextLevel;
    public event Action<float, float> OnExpChanged;
    public event Action<int> OnLevelUp;

    [Header("Character Bonus Stats")]
    [SyncVar] protected float bonusStamina = 0;
    [SyncVar] protected float bonusCriticalChance = 0;
    [SyncVar] protected float bonusCriticalDamage = 0;
    public float BonusStamina => bonusStamina;
    public float BonusCriticalChance => bonusCriticalChance;
    public float BonusCriticalDamage => bonusCriticalDamage;

    [Header("Total Stats")]
    protected float totalStamina => characterData.Stamina + bonusStamina + (2f * vitality);
    protected float totalCriticalChance => characterData.CriticalChance + bonusCriticalChance + (0.4f * luck);
    protected float totalCriticalDamage => characterData.CriticalDamageMultiplier + bonusCriticalDamage;
    public float TotalStamina => totalStamina;
    public float TotalCriticalChance => totalCriticalChance;
    public float TotalCriticalDamage => totalCriticalDamage;

    [SyncVar(hook = nameof(StaminaSync))] protected float stamina;
    public float Stamina => stamina;
    public event System.Action<float> OnStaminaChanged;
    protected float _staminaRegenRate;
    protected float _staminaConsumptionMultiplier;
    private Coroutine _staminaRegenCoroutine;

    private Inventory playerInventory;
    public Inventory PlayerInventory => playerInventory;

    private CurrencyWallet currencyWallet;
    public CurrencyWallet CurrencyWallet => currencyWallet;

    private Equipment equipment;
    public Equipment PlayerEquipment => equipment;

    public override void Init()
    {
        base.Init();
        stamina = totalStamina;
        _staminaRegenRate = characterData.StaminaRegenRate;
        _staminaConsumptionMultiplier = characterData.StaminaConsumptionMultiplier;
        OnStaminaChanged?.Invoke(stamina);

        this.playerInventory = new Inventory();
        currencyWallet = new CurrencyWallet();
        equipment = new Equipment(this);
    }

    private void StaminaSync(float oldValue, float newValue)
    {
        OnStaminaChanged?.Invoke(newValue);
    }

    public virtual void GainExp(float amount)
    {
        if (amount <= 0) return;

        currentExp += amount;
        OnExpChanged?.Invoke(currentExp, expToNextLevel);

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }
    }

    LocalizedString localizedText = new LocalizedString("UI", "UI_LevelUp");
    protected virtual void LevelUp()
    {
        level++;
        OnLevelUp?.Invoke((int)level);

        expToNextLevel = Mathf.Round(expToNextLevel * 1.25f);

        hp = TotalHealth;
        InvokeHPChanged(hp);

        UIManager.Instance.GetSingleUIService().Create
            ("FloatingText", Guid.NewGuid().ToString(), localizedText, transform.position + Vector3.up * 1.1f, true);

        Debug.Log($"{gameObject.name} leveled up to {level}!");
    }

    public override void ApplyAttributes(CharacterAttributes attributes)
    {
        base.ApplyAttributes(attributes);

        if (_staminaRegenCoroutine != null)
        {
            StopCoroutine(_staminaRegenCoroutine);
        }
        _staminaRegenCoroutine = StartCoroutine(RegenStamina());
    }

    public override void ApplyBonusStat(BonusStat bonusStat, float amount)
    {
        base.ApplyBonusStat(bonusStat, amount);
        switch (bonusStat)
        {
            case BonusStat.Stamina:
                bonusStamina = amount; break;
            case BonusStat.CriticalChance:
                bonusCriticalChance = amount; break;
            case BonusStat.CriticalDmg:
                bonusCriticalDamage = amount; break;
        }
    }

    public bool UseStamina(float amount)
    {
        float adjustedAmount = amount * _staminaConsumptionMultiplier;
        if (stamina >= adjustedAmount)
        {
            stamina -= adjustedAmount;
            OnStaminaChanged?.Invoke(stamina);

            if (_staminaRegenCoroutine != null)
            {
                StopCoroutine(_staminaRegenCoroutine);
            }
            _staminaRegenCoroutine = StartCoroutine(RegenStamina());

            return true;
        }
        return false;
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2f);
        while (stamina < totalStamina)
        {
            stamina += _staminaRegenRate * Time.deltaTime; 
            if (stamina > totalStamina)
            {
                stamina = totalStamina;
            }
            OnStaminaChanged?.Invoke(stamina);
            yield return null;
        }
        _staminaRegenCoroutine = null;
    }
}
