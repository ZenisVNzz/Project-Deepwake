using UnityEngine;
using UnityEngine.InputSystem;

public class CannonBulletRuntime : BulletRuntime
{
    private Rigidbody2D rb;
    private float speed = 6f;
    private Vector2 dir;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 dir)
    {
        rb.linearVelocity = dir * speed;
    }
}