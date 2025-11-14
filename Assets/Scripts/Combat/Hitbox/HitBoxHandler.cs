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
        if (!isServer && !isOwned)
            return;

        if (other.CompareTag(undamagedTag) || other.CompareTag("Untagged"))
            return;

        IAttackable damageable = other.GetComponentInParent<IAttackable>();
        if (damageable == null) return;

        Vector3 knockDir = (other.transform.position - transform.position).normalized * knockbackForce;
        float finalDamage = isOwnerDmg ? _characterRuntime.TotalAttack : damage;

        if (isServer)
        {
            DealDamageInternal(damageable, finalDamage, knockDir);
        }
        else if (isOwned)
        {
            CmdDealDamage(((MonoBehaviour)damageable).gameObject, finalDamage, knockDir);
        }

        Debug.Log($"[HitBox] {gameObject.name} dealt {finalDamage} dmg to {other.name}");
    }

    [Command]
    private void CmdDealDamage(GameObject targetObj, float damageAmount, Vector3 knockback)
    {
        if (targetObj == null) return;

        IAttackable target = targetObj.GetComponentInParent<IAttackable>();
        if (target == null) return;

        DealDamageInternal(target, damageAmount, knockback);
    }

    [Server]
    private void DealDamageInternal(IAttackable target, float damageAmount, Vector3 knockback)
    {
        if (_characterRuntime != null)
            target.TakeDamage(damageAmount, knockback, _characterRuntime);
        else
            target.TakeDamage(damageAmount, knockback);

        if (destroyThisOnHit)
            NetworkServer.Destroy(gameObject);
    }
}
