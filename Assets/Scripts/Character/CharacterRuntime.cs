using System;
using System.Collections;
using UnityEngine;

public class CharacterRuntime : MonoBehaviour, ICharacterRuntime
{
    [SerializeField] protected float hp;
    public float HP => hp;

    protected float _hpRegenRate;

    protected CharacterData characterData;
    public CharacterData CharacterData => characterData;

    protected Rigidbody2D rb;
    protected IState characterState;

    private Material flashMaterial;
    private DamageFlash damageFlash;

    public virtual void Init(CharacterData CharacterData, Rigidbody2D rigidbody2D, IState characterState)
    {
        characterData = Instantiate(CharacterData);
        hp = CharacterData.HP;

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
                ("FloatingDamage", $"FloatingDamage{Time.time}", FinalDamage.ToString("F1"), transform.position + Vector3.up * 0.8f);

            hp -= FinalDamage;
            rb.AddForce(knockback, ForceMode2D.Impulse);
            characterState.ChangeState(CharacterStateType.Knockback);

            if (hp <= 0)
            {
                Die();
            }

            Debug.Log($"{gameObject} took {FinalDamage} damage, remaining HP: {hp}");
        }   
    }

    protected void Die()
    {
        Debug.Log($"{gameObject} has died.");
        characterState.ChangeState(CharacterStateType.Death);
    }
}
