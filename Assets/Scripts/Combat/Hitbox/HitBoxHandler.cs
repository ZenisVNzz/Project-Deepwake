using Mirror;
using UnityEngine;

public class HitBoxHandler : NetworkBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private string undamagedTag;

    [SerializeField] private bool destroyThisOnHit = false;
    public CharacterRuntime _characterRuntime = null;

    public bool isOwnerDmg = false;

    public void SetData(float damage, string undamagedTag, CharacterRuntime owner, float knockbackForce = 10f)
    {
        this.damage = damage;     
        this.undamagedTag = undamagedTag;      
        _characterRuntime = owner;
        this.knockbackForce = knockbackForce;
    }

    public void SetData(string undamagedTag, CharacterRuntime owner, float knockbackForce = 10f)
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

                if (damageable is EnemyRuntime enemyRuntime)
                {
                    CmdDealDamage(enemyRuntime.gameObject, isOwnerDmg ? _characterRuntime.TotalAttack : damage, knockbackDirection * knockbackForce);
                    Debug.Log($"Dealt {damage} damage with {knockbackForce} knockback to {enemyRuntime.gameObject.name}");
                }                   
            }
        }    
    }

    [Command]
    private void CmdDealDamage(GameObject targetObj, float damageAmount, Vector3 knockback)
    {
        if (targetObj == null) return;

        IAttackable target = targetObj.GetComponentInParent<IAttackable>();
        if (target != null)
        {
            if (_characterRuntime == null)
            {
                target.TakeDamage(damageAmount, knockback);
                
            }
            else
            {
                target.TakeDamage(damageAmount, knockback, _characterRuntime);
            }       

            if (destroyThisOnHit)
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
