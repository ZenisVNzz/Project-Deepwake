using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashForce = 5f;     
    public float dashDuration = 0.2f;  
    public float dashCooldown = 1f;     

    private Rigidbody2D rb;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashTime;
    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDashing && Time.time > dashTime)
        {
            isDashing = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void Dash()
    {
        Debug.Log("Dash");
        if (canDash && !isDashing)
        {
            dashDirection = rb.linearVelocity.normalized;
            rb.linearVelocity = dashDirection * dashForce;

            isDashing = true;
            dashTime = Time.time + dashDuration;

            canDash = false;
            Invoke(nameof(ResetDash), dashCooldown);
        }
    }

    void ResetDash()
    {
        canDash = true;
    }
}