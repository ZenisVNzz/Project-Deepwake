using UnityEngine;

public class HitBoxHandler : MonoBehaviour
{
    [Header("HitBox Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float knockbackForce = 5f;

    private void OnTriggerEnter(Collider other)
    {
        IAttackable damageable = other.GetComponent<IAttackable>();
        if (damageable != null)
        {
            Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
            damageable.TakeDamage(damage, knockbackDirection * knockbackForce);
        }
    }
}
