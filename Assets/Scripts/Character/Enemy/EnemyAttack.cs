using UnityEngine;

public class EnemyAttack : IDamageDealer
{
    private IState _enemyState;
    private HitBoxController _hitBoxController;

    private float _cooldown;
    private float _nextAttackTime;

    public EnemyAttack(IState playerState, HitBoxController hitBoxController, float attackCooldown = 4f)
    {
        _enemyState = playerState;
        _hitBoxController = hitBoxController;
        _cooldown = Random.Range(attackCooldown * 0.8f, attackCooldown * 1.2f);
        _nextAttackTime = 0f;
    }

    public void Attack(float ATK)
    {
        if (Time.time < _nextAttackTime || _enemyState.GetCurrentState() == CharacterStateType.Attacking)
        {
            return;
        }

        if (_hitBoxController != null)
        {
            _hitBoxController.SetStats(ATK, 8f);
        }
        
        CharacterStateType state = _enemyState.GetCurrentState();
        if (state != CharacterStateType.Attacking)
        {
            _enemyState.ChangeState(CharacterStateType.Attacking);
            _nextAttackTime = Time.time + _cooldown;
        }
    }
}
