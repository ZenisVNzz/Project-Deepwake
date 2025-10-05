using UnityEngine;

[System.Serializable]
public class PlayerMovement : IMovable
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 20f;

    private Rigidbody2D rb;
    private Vector2 input;

    private IState playerState;

    public PlayerMovement(Rigidbody2D rigidbody, IState playerState)
    {
        this.rb = rigidbody;
        this.playerState = playerState;
    }

    public void Move(Vector2 input)
    {
        this.input = input;

        if (playerState.GetCurrentState() == CharacterStateType.Attacking)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            return;
        }

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, input.normalized * moveSpeed, acceleration * Time.fixedDeltaTime);
    }

    public Vector2 GetDir()
    {
        return input;
    }
}