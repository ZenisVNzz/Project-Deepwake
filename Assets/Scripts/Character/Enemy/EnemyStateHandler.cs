using System.Threading.Tasks;
using UnityEngine;

public class EnemyStateHandler : IStateHandler
{
    private IState enemyState;
    private Rigidbody2D rb;

    public EnemyStateHandler(IState state, Rigidbody2D rigidbody2D)
    {
        enemyState = state;
        this.rb = rigidbody2D;
    }

    public async void UpdateState()
    {
        if (enemyState.GetCurrentState() == CharacterStateType.Attacking)
        {
            return;
        }
        else if (enemyState.GetCurrentState() == CharacterStateType.Knockback)
        {
            await WaitForKnockBack();
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
        return rb.linearVelocity.sqrMagnitude > 0.01f;
    }

    private async Task WaitForKnockBack()
    {
        await Task.Delay(700);
        enemyState.ChangeState(CharacterStateType.Idle);
    }
}
