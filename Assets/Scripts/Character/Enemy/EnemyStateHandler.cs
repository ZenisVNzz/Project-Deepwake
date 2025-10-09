using UnityEngine;

public class EnemyStateHandler : IStateHandler
{
    private IState enemyState;
    private IMovable enemyMovement;

    public EnemyStateHandler(IState state, IMovable enemyMovement)
    {
        enemyState = state;
        this.enemyMovement = enemyMovement;
    }

    public void UpdateState()
    {
        if (enemyState.GetCurrentState() == CharacterStateType.Attacking)
        {
            return;
        }
        else
        {
            if (CheckIfMoving())
                enemyState.ChangeState(CharacterStateType.Running);
            else
                enemyState.ChangeState(CharacterStateType.Idle);
        }
    }

    private bool CheckIfMoving()
    {
        Vector2 vec = enemyMovement.GetDir();

        if (vec.sqrMagnitude > 0.01f)
            return true;
        else
            return false;
    }
}
