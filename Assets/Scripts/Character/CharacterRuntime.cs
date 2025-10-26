using System;
using UnityEngine;

public class CharacterRuntime : MonoBehaviour, ICharacterRuntime
{
    [Header("Character Attributes")]
    [SerializeField] private float Level = 1;
    [SerializeField] private float Vitality = 1;
    [SerializeField] private float Defense = 1;
    [SerializeField] private float Strength = 1;
    [SerializeField] private float Luck = 1;

    [Header("Character Bonus Stats")]
    protected float bonusMaxHealth = 0;
    protected float bonusStamina = 0;
    protected float bonusAttackPower = 0;
    protected float bonusDefense = 0;
    protected float bonusSpeed = 0;
    protected float bonusCriticalChance = 0;
    protected float bonusCriticalDamage = 0;

    [Header("Total Stats")]
    protected float totalHealth => characterData.HP + bonusMaxHealth + (5f * Vitality);
    protected float totalStamina => characterData.Stamina + bonusStamina + (2f * Vitality);
    protected float totalAttack => characterData.AttackPower + bonusAttackPower + (2f * Strength);
    protected float totalDefense => characterData.Defense + bonusDefense + (1f * Defense);
    protected float totalSpeed => characterData.MoveSpeed + bonusSpeed;
    protected float totalCriticalChance => characterData.CriticalChance + bonusCriticalChance + (0.01f * Luck);
    protected float totalCriticalDamage => characterData.CriticalDamageMultiplier + bonusCriticalDamage;
    public float TotalHealth => totalHealth;
    public float TotalStamina => totalStamina;
    public float TotalAttack => totalAttack;
    public float TotalDefense => totalDefense;
    public float TotalSpeed => totalSpeed;
    public float TotalCriticalChance => totalCriticalChance;
    public float TotalCriticalDamage => totalCriticalDamage;

    [Header("Reference")]
    protected Action _onStatusChanged;
    public event Action OnStatusChanged
    {
        add { _onStatusChanged += value; }
        remove { _onStatusChanged -= value; }
    }

    [SerializeField] protected float hp;
    public float HP => hp;
    protected float _hpRegenRate;

    [SerializeField] protected float stamina;
    public float Stamina => stamina;
    protected float _staminaRegenRate;
    protected float _staminaConsumptionMultiplier;

    protected CharacterData characterData;
    public CharacterData CharacterData => characterData;

    protected Rigidbody2D rb;
    protected IState characterState;

    private Material flashMaterial;
    private DamageFlash damageFlash;

    public virtual void Init(CharacterData CharacterData, Rigidbody2D rigidbody2D, IState characterState)
    {
        characterData = CharacterData;
        hp = totalHealth;
        stamina = totalStamina;
        _hpRegenRate = characterData.HPRegenRate;
        _staminaRegenRate = characterData.StaminaRegenRate;
        _staminaConsumptionMultiplier = characterData.StaminaConsumptionMultiplier;

        rb = rigidbody2D;  
        this.characterState = characterState;
    }

    public virtual void TakeDamage(float damage, Vector3 knockback)
    {
        if (characterState.GetCurrentState() != CharacterStateType.Death)
        {
            if (this == null) return;

            DamageReduceCal damageReduceCal = new DamageReduceCal();
            float FinalDamage = damageReduceCal.Calculate(damage, characterData.Defense);

            if (flashMaterial == null)
            {
                flashMaterial = ResourceManager.Instance.GetAsset<Material>("DamageFlashMaterial");
                damageFlash = new DamageFlash(GetComponent<SpriteRenderer>(), flashMaterial);
            }

            damageFlash.TriggerFlash();
            UIManager.Instance.GetSingleUIService().Create
                ("FloatingDamage", $"FloatingDamage{Time.time}_{UnityEngine.Random.Range(0, 99999)}", FinalDamage.ToString("F1"), transform.position + Vector3.up * 0.8f);

            hp -= FinalDamage;   
            _onStatusChanged?.Invoke();

            if (hp <= 0)
            {
                Die();
            }

            if (characterState.GetCurrentState() != CharacterStateType.Attacking && characterState.GetCurrentState() != CharacterStateType.Death)
            {
                rb.AddForce(knockback, ForceMode2D.Impulse);
                characterState.ChangeState(CharacterStateType.Knockback);
            }

            Debug.Log($"{gameObject} took {FinalDamage} damage, remaining HP: {hp}");
        }   
    }

    protected void Die()
    {
        characterState.ChangeState(CharacterStateType.Death);
        Debug.Log($"{gameObject} has died.");
    }
}
