using Mirror;
using System;
using System.Collections;
using UnityEngine;

public enum BonusStat
{
    Health,
    AttackPower,
    Defense,
    Speed,
    Stamina,
    CriticalChance,
    CriticalDmg
}

public class CharacterRuntime : NetworkBehaviour, ICharacterRuntime
{
    [Header("Character Attributes")]
    [SerializeField] protected float level = 1;
    [SerializeField] protected float vitality = 0;
    [SerializeField] protected float defense = 0;
    [SerializeField] protected float strength = 0;
    [SerializeField] protected float luck = 0;
    public float Level => level;
    public float Vitality => vitality;
    public float Defense => defense;
    public float Strength => strength;
    public float Luck => luck;

    [Header("Character Bonus Stats")]
    protected float bonusMaxHealth = 0;  
    protected float bonusAttackPower = 0;
    protected float bonusDefense = 0;
    protected float bonusSpeed = 0;
    public float BonusMaxHealth => bonusMaxHealth;
    public float BonusAttackPower => bonusAttackPower;
    public float BonusDefense => bonusDefense;
    public float BonusSpeed => bonusSpeed;

    [Header("Total Stats")]
    [SerializeField] protected float totalHealth => characterData.HP + bonusMaxHealth + (5f * vitality);
    [SerializeField] protected float totalAttack => characterData.AttackPower + bonusAttackPower + (2f * strength);
    [SerializeField] protected float totalDefense => characterData.Defense + bonusDefense + (1f * defense);
    [SerializeField] protected float totalSpeed => characterData.MoveSpeed + bonusSpeed;
    public float TotalHealth => totalHealth;
    public float TotalAttack => totalAttack;
    public float TotalDefense => totalDefense;
    public float TotalSpeed => totalSpeed;    

    [SerializeField] protected float hp;
    public float HP => hp;
    public event Action<float> OnHPChanged;
    protected float _hpRegenRate;   

    protected CharacterData characterData;

    protected Rigidbody2D rb;
    protected IState characterState;

    private Material flashMaterial;
    private DamageFlash damageFlash;

    public CharacterAttributes RuntimeAttributes { get; private set; } = new CharacterAttributes();

    private Coroutine _hpRegenCoroutine;

    private PlayerNet PlayerNet => NetworkClient.connection.identity.GetComponent<PlayerNet>();

    public virtual void Init()
    {
        characterData = GetComponent<CharacterInstaller>()._characterData;
        hp = totalHealth;
        _hpRegenRate = characterData.HPRegenRate;    
        OnHPChanged?.Invoke(hp);

        rb = GetComponent<Rigidbody2D>();
        this.characterState = GetComponent<PlayerController>().playerState;
    }

    public virtual void ApplyAttributes(CharacterAttributes attributes)
    {
        RuntimeAttributes = attributes;
        vitality = attributes.VIT;
        defense = attributes.DEF;
        strength = attributes.STR;
        luck = attributes.LUCK;

        if (hp > TotalHealth) hp = TotalHealth;
        OnHPChanged?.Invoke(hp);
        if (_hpRegenCoroutine != null)
        {
            StopCoroutine(_hpRegenCoroutine);
        }
        _hpRegenCoroutine = StartCoroutine(RegenHP());
    }

    public virtual void ApplyBonusStat(BonusStat bonusStat, float amount)
    {
        switch (bonusStat)
        {
            case BonusStat.Health:
                bonusMaxHealth = amount; break;
            case BonusStat.Defense:
                bonusDefense = amount; break;
            case BonusStat.Speed:
                bonusSpeed = amount; break;
            case BonusStat.AttackPower:
                bonusAttackPower = amount; break;
        }
    }

    public virtual void TakeDamage(float damage, Vector3 knockback, ICharacterRuntime characterRuntime)
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
            OnHPChanged?.Invoke(hp);
            if (_hpRegenCoroutine != null)
            {
                StopCoroutine(_hpRegenCoroutine);
            }
            _hpRegenCoroutine = StartCoroutine(RegenHP());

            if (hp <= 0)
            {
                Die();
                if (characterRuntime is IPlayerRuntime playerRuntime)
                {
                    playerRuntime.GainExp(characterData.ExpOnKill);
                    playerRuntime.CurrencyWallet.Add(CurrencyType.Gold, characterData.GoldOnKill);
                }
            }

            if (characterState.GetCurrentState() != CharacterStateType.Attacking && characterState.GetCurrentState() != CharacterStateType.Death)
            {
                rb.AddForce(knockback, ForceMode2D.Impulse);
                PlayerNet.CmdChangeState(CharacterStateType.Knockback);
            }

            Debug.Log($"{gameObject} took {FinalDamage} damage, remaining HP: {hp}");
        }   
    }

    public virtual void TakeDamage(float damage, Vector3 knockback)
    {
        TakeDamage(damage, knockback, null);
    }

    protected void Die()
    {
        PlayerNet.CmdChangeState(CharacterStateType.Death);
        Debug.Log($"{gameObject} has died.");
    }

    private IEnumerator RegenHP()
    {
        yield return new WaitForSeconds(2f);
        while (hp < totalHealth)
        {
            hp += _hpRegenRate * Time.deltaTime;
            if (hp > totalHealth)
            {
                hp = totalHealth;
            }
            OnHPChanged?.Invoke(hp);
            yield return null;
        }
        _hpRegenCoroutine = null;
    }

    protected void InvokeHPChanged(float newHP)
    {
        OnHPChanged?.Invoke(newHP);
    }
}
