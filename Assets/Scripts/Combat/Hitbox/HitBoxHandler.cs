using UnityEngine;

public class HitBoxHandler : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private string undamagedTag;

    [SerializeField] private bool destroyThisOnHit = false;
    private ICharacterRuntime _characterRuntime;

    private bool isOwnerDmg = false;

    public void SetData(float damage, string undamagedTag, ICharacterRuntime owner, float knockbackForce = 10f)
    {
        this.damage = damage;     
        this.undamagedTag = undamagedTag;      
        _characterRuntime = owner;
        this.knockbackForce = knockbackForce;
    }

    public void SetData(string undamagedTag, ICharacterRuntime owner, float knockbackForce = 10f)
    {
        this.undamagedTag = undamagedTag;
        _characterRuntime = owner;
        this.knockbackForce = knockbackForce;
        isOwnerDmg = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag != undamagedTag && other.tag != "Untagged")
        {
            IAttackable damageable = other.GetComponentInParent<IAttackable>();
            if (damageable != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;

                if (damageable is EnemyRuntime enemyRuntime && _characterRuntime is IPlayerRuntime playerRuntime)
                {
                    damageable.TakeDamage(isOwnerDmg? _characterRuntime.TotalAttack : damage, knockbackDirection * knockbackForce, _characterRuntime);
                }
                else
                {
                    damageable.TakeDamage(isOwnerDmg ? _characterRuntime.TotalAttack : damage, knockbackDirection * knockbackForce, null);
                }

                if (destroyThisOnHit)
                {
                    Destroy(gameObject);
                }

                Debug.Log($"Dealt {damage} damage with {knockbackForce} knockback to {other.name}");
            }
        }    
    }
}
