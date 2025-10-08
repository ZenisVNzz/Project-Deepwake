using UnityEngine;

public class Wolf : MonoBehaviour, IAttackable
{
    [SerializeField] private float health = 100f;
    [SerializeField] private Material flashMaterial;
    private Rigidbody2D rb;

    private DamageFlash damageFlash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageFlash = new DamageFlash(GetComponent<SpriteRenderer>(), flashMaterial);
    }

    public void TakeDamage(float damage, Vector3 knockback)
    {
        if (this == null) return;

        health -= damage;
        rb.AddForce(knockback, ForceMode2D.Impulse);
        damageFlash.TriggerFlash();
        UIManager.Instance.GetSingleUIService().Create("FloatingDamage", $"FloatingDamage{Time.time}", damage.ToString(), transform.position + Vector3.up * 0.8f);

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
