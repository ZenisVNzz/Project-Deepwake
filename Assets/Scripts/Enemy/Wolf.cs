using UnityEngine;

public class Wolf : MonoBehaviour, IAttackable
{
    [SerializeField] private float health = 100f;

    public void TakeDamage(float damage, Vector3 knockback)
    {
        health -= damage;
        Debug.Log($"Wolf took {damage} damage. Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Wolf has died.");
        Destroy(gameObject);
    }
}
