using UnityEngine;

public class Wolf : MonoBehaviour, IAttackable
{
    [SerializeField] private float health = 100f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage, Vector3 knockback)
    {
        if (this == null) return;

        health -= damage;
        rb.AddForce(knockback, ForceMode2D.Impulse);

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
