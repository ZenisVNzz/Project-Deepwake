using UnityEngine;

public class EnemyAttack : MonoBehaviour, IDamageDealer
{
    private float attackCooldown = 5f;

    private IState _enemyState;
    private HitBoxController _hitBoxController;

    private float _cooldown;
    private float _nextAttackTime;

    private void Awake()
    {
        _enemyState = GetComponent<IState>();
        _hitBoxController = GetComponent<HitBoxController>();
        _cooldown = Random.Range(attackCooldown * 0.8f, attackCooldown * 1.8f);
        _nextAttackTime = 0f;
    }

    public void Attack(float ATK)
    {
        if (Time.time < _nextAttackTime || _enemyState.GetCurrentState() == CharacterStateType.Attacking)
        {
            return;
        }
        
        CharacterStateType state = _enemyState.GetCurrentState();
        if (state != CharacterStateType.Attacking)
        {
            _enemyState.ChangeState(CharacterStateType.Attacking);
            _nextAttackTime = Time.time + _cooldown;
        }
    }
}
