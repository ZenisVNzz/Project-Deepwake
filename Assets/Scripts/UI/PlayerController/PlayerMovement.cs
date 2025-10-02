using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Joystick joystick;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        Vector2 move = new Vector2(moveX, moveY);

        rb.linearVelocity = move * moveSpeed;

        if (move != Vector2.zero)
        {
            float angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }
}