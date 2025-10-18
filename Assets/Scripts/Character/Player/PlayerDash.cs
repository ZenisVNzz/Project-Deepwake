using System.Threading.Tasks;
using UnityEngine;

public class PlayerDash : IDashable
{
    private float _dashForce = 15f;
    private float _dashDuration = 0.2f;   
    private float _staminaCost = 25f;

    private Rigidbody2D _rb;
    private bool _isDashing = false;
    private float _dashTime;
    private Vector2 _dashDirection;

    private IPlayerRuntime _characterRuntime;

    public PlayerDash(Rigidbody2D rigidbody, IPlayerRuntime characterRuntime)
    {
        _rb = rigidbody;
        this._characterRuntime = characterRuntime;
    }

    private async Task DaskCooldown()
    {
        while (_isDashing && Time.time <= _dashTime)
        {
            await Task.Yield();
        }
        _isDashing = false;
    }

    public async void Dash()
    {
        if (!_isDashing && _characterRuntime.UseStamina(_staminaCost))
        {
            _dashDirection = _rb.linearVelocity.normalized;
            _rb.linearVelocity = _dashDirection * _dashForce;

            _isDashing = true;
            _dashTime = Time.time + _dashDuration;
            await DaskCooldown();
        }
    }
}