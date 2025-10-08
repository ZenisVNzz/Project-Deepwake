using System.Collections;
using UnityEngine;

public class CharacterRuntime : MonoBehaviour, ICharacterRuntime
{
    [SerializeField] protected float _hp;
    public float HP => _hp;

    protected float _hpRegenRate;

    protected CharacterData _characterData;
    public CharacterData CharacterData => _characterData;

    protected Rigidbody2D _rigidbody2D;

    private Material flashMaterial;
    private DamageFlash damageFlash;

    public virtual void Init(CharacterData CharacterData, Rigidbody2D rigidbody2D)
    {
        _characterData = Instantiate(CharacterData);
        _hp = CharacterData.HP;

        _rigidbody2D = rigidbody2D;     
    }

    public virtual void TakeDamage(float damage, Vector3 knockback)
    {
        if (this == null) return;

        DamageReduceCal damageReduceCal = new DamageReduceCal();
        float FinalDamage = damageReduceCal.Calculate(damage, _characterData.Defense);

        if (flashMaterial == null)
        {
            flashMaterial = ResourceManager.Instance.GetAsset<Material>("DamageFlashMaterial");
            damageFlash = new DamageFlash(GetComponent<SpriteRenderer>(), flashMaterial);
        }

        damageFlash.TriggerFlash();
        UIManager.Instance.GetSingleUIService().Create("FloatingDamage", $"FloatingDamage{Time.time}", FinalDamage.ToString(), transform.position + Vector3.up * 0.8f);

        _hp -= FinalDamage;
        _rigidbody2D.AddForce(knockback, ForceMode2D.Impulse);

        if (_hp <= 0)
        {
            Die();
        }

        Debug.Log($"{gameObject} took {FinalDamage} damage, remaining HP: {_hp}");
    }

    protected void Die()
    {
        Debug.Log($"{gameObject} has died.");
    }
}
