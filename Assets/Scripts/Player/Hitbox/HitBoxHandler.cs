using UnityEngine;

public class HitBoxHandler : MonoBehaviour
{
    [Header("HitBox Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float knockbackForce = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (other.CompareTag("Enemy"))
        {
            IAttackable damageable = other.GetComponentInParent<IAttackable>();
            if (damageable != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                damageable.TakeDamage(damage, knockbackDirection * knockbackForce);
                Debug.Log($"Dealt {damage} damage with {knockbackForce} knockback to {other.name}");
            }
        }    
    }
}
