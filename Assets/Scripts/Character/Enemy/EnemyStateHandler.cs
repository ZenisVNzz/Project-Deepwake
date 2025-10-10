using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyStateHandler : IStateHandler
{
    private IState enemyState;
    private Rigidbody2D rb;

    private bool IsWaitForKnockBack = false;

    public EnemyStateHandler(IState state, Rigidbody2D rigidbody2D)
    {
        enemyState = state;
        this.rb = rigidbody2D;
    }

    public void UpdateState()
    {
        if (enemyState.GetCurrentState() == CharacterStateType.Attacking || enemyState.GetCurrentState() == CharacterStateType.Death)
        {
            return;
        }
        else if (enemyState.GetCurrentState() == CharacterStateType.Knockback)
        {
            if (!IsWaitForKnockBack)
            {
                CoroutineRunner.Instance.RunCoroutine(WaitForKnockBack());
            }
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
        return rb.linearVelocity.sqrMagnitude > 0.05f;
    }

    private IEnumerator WaitForKnockBack()
    {
        IsWaitForKnockBack = true;
        yield return new WaitForSeconds(0.7f);
        if (enemyState.GetCurrentState() == CharacterStateType.Knockback)
        {
            enemyState.ChangeState(CharacterStateType.Idle);
            IsWaitForKnockBack = false;
        }  
    }
}
