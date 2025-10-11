using UnityEngine;

public class HitBoxHandler : MonoBehaviour
{
    private HitBoxController _controller;

    public void Init(HitBoxController hitBoxController)
    {
        _controller = hitBoxController;
    }    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != _controller.tag && other.tag != gameObject.tag)
        {
            IAttackable damageable = other.GetComponentInParent<IAttackable>();
            if (damageable != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                damageable.TakeDamage(_controller.Damage, knockbackDirection * _controller.KnockbackForce);
                Debug.Log($"Dealt {_controller.Damage} damage with {_controller.KnockbackForce} knockback to {other.name}");
            }
        }    
    }
}
