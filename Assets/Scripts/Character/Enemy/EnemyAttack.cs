using UnityEngine;

public class EnemyAttack : IDamageDealer
{
    private IState _enemyState;
    private HitBoxController _hitBoxController;

    public EnemyAttack(IState playerState, HitBoxController hitBoxController)
    {
        _enemyState = playerState;
        _hitBoxController = hitBoxController;
    }

    public void Attack()
    {
        CharacterStateType state = _enemyState.GetCurrentState();
        if (state != CharacterStateType.Attacking)
        {
            _enemyState.ChangeState(CharacterStateType.Attacking);
        }
    }
}
