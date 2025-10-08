using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 20f;

    private Joystick joystick;
    private Rigidbody2D rb;
    private Vector2 input;

    public PlayerMovement(Rigidbody2D rigidbody, Joystick joystick)
    {
        this.rb = rigidbody;
        this.joystick = joystick;
    }

    public void Move()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        input = new Vector2(moveX, moveY);

        rb.linearVelocity = input * moveSpeed;
    }

    public Vector2 GetDir()
    {
        return input;
    }
}