using Mirror;
using UnityEngine;

[System.Serializable]
public class PlayerMovement : NetworkBehaviour, IMovable
{
    [Header("Movement Settings")]
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 20f;

    private Rigidbody2D rb;
    private Vector2 input;
    [SyncVar] private Vector2 moveVelocity;

    private IState playerState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerController>().playerState;
    }

    [Command]
    public void CmdMove(Vector2 input, float moveSpeed, bool isMoveOnSlope)
    {
        this.input = input;

        if (playerState.GetCurrentState() == CharacterStateType.Attacking)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            return;
        }

        if (isMoveOnSlope)
        {
            MoveAlongSlope(new Vector2(1, 1), moveSpeed, 0.7f);
        }
        else
        {
            Vector2 isoInput = ToIsometric(input.normalized);
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, isoInput * moveSpeed, acceleration * Time.fixedDeltaTime);
        }   
        moveVelocity = rb.linearVelocity;
    }

    [Command]
    public void Move(float moveSpeed) => Debug.LogWarning("[PlayerMovement] input is missing.");

    private Vector2 ToIsometric(Vector2 input)
    {
        float isoX = input.x;
        float isoY = input.y * 0.5f;
        Vector2 iso = new Vector2(isoX, isoY);

        return iso.normalized;
    }

    [Server]
    private void MoveAlongSlope(Vector2 dir, float moveSpeed, float speedModifier)
    {
        const float epsilon = 0.0001f;

        if (input.sqrMagnitude <= epsilon)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            return;
        }

        Vector2 isoInput = ToIsometric(input.normalized);
        Vector2 move = Vector2.Lerp(rb.linearVelocity, (isoInput * moveSpeed) * speedModifier, acceleration * Time.fixedDeltaTime);

        if (move.x < 0)
        {
            rb.linearVelocity = new Vector2(move.x, move.y + dir.y * 0.32f);
        }
        else
        {
            rb.linearVelocity = new Vector2(move.x, move.y - dir.y * 0.32f);
        }
    }

    public Vector2 GetDir()
    {
        return moveVelocity.normalized;
    }
}