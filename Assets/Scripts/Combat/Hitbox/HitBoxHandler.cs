using UnityEngine;

public class HitBoxHandler : MonoBehaviour
{
    private HitBoxController _controller;
    [SerializeField] private bool isControllerDepend;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private string undamagedTag;

    [SerializeField] private bool destroyThisOnHit = false;

    public void Init(HitBoxController hitBoxController)
    {
        _controller = hitBoxController;
    }    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isControllerDepend)
        {
            undamagedTag = gameObject.tag;
            damage = _controller.Damage;
            knockbackForce = _controller.KnockbackForce;
        }

        if (other.tag != undamagedTag && other.tag != "Untagged")
        {
            IAttackable damageable = other.GetComponentInParent<IAttackable>();
            if (damageable != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                damageable.TakeDamage(damage, knockbackDirection * knockbackForce);

                if (destroyThisOnHit)
                {
                    Destroy(gameObject);
                }

                Debug.Log($"Dealt {damage} damage with {knockbackForce} knockback to {other.name}");
            }
        }    
    }
}
