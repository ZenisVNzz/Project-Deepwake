using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float damage = 8f;
    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody2D rb;
    private Vector2 dir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (lifeTime > 0f)
        {
            Destroy(gameObject, lifeTime);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetStats(float speed, float damage, float knockback)
    {
        this.speed = speed;
        this.damage = damage;
        this.knockbackForce = knockback;
    }

    public void Init(Vector2 direction)
    {
        dir = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.right;
        if (rb != null)
        {
            rb.linearVelocity = dir * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.gameObject == gameObject)
            return;

        var attackable = other.GetComponent<IAttackable>();
        if (attackable != null)
        {
            Vector3 knockback = (Vector3)(dir * knockbackForce);
            attackable.TakeDamage(damage, knockback);
            Destroy(gameObject);
            return;
        }

        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
