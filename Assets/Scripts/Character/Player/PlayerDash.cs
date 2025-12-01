using System.Threading.Tasks;
using UnityEngine;

public class PlayerDash : MonoBehaviour, IDashable
{
    private float _dashForce = 15f;
    private float _dashDuration = 0.2f;   
    private float _staminaCost = 25f;

    private Rigidbody2D _rb;
    private bool _isDashing = false;
    private float _dashTime;
    private Vector2 _dashDirection;

    private IPlayerRuntime _characterRuntime;
    private GhostTrail _ghostTrail;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _characterRuntime = GetComponent<IPlayerRuntime>();
        _ghostTrail = GetComponent<GhostTrail>();
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
            if (_ghostTrail != null)
            {
                _ghostTrail.StartTrail(0.2f);
            }
            _dashDirection = _rb.linearVelocity.normalized;
            _rb.linearVelocity = _dashDirection * _dashForce;

            _isDashing = true;
            _dashTime = Time.time + _dashDuration;
            await DaskCooldown();
        }
    }
}