using System.Threading.Tasks;
using UnityEngine;

public class PlayerDash : IDashable
{
    public float dashForce = 15f;     
    public float dashDuration = 0.2f;  
    public float dashCooldown = 1f;     

    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashTime;
    private Vector2 dashDirection;

    public PlayerDash(Rigidbody2D rigidbody)
    {
        rb = rigidbody;
    }

    private async Task DaskCooldown()
    {
        while (isDashing && Time.time <= dashTime)
        {
            await Task.Yield();
        }
        isDashing = false;
    }

    public async void Dash()
    {
        if (!isDashing)
        {
            dashDirection = rb.linearVelocity.normalized;
            rb.linearVelocity = dashDirection * dashForce;

            isDashing = true;
            dashTime = Time.time + dashDuration;
            await DaskCooldown();
        }
    }
}