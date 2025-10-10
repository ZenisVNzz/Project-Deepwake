using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerStateHandler : IStateHandler
{
    private IState playerState;
    private Rigidbody2D rb;
    private InputSystem_Actions inputHandler;

    private bool IsWaitForKnockBack = false;

    public PlayerStateHandler(IState state, Rigidbody2D rigidbody2D, InputSystem_Actions inputHandler)
    {
        playerState = state;
        rb = rigidbody2D;
        this.inputHandler = inputHandler;
    }

    public void UpdateState()
    {
        if (playerState.GetCurrentState() == CharacterStateType.Attacking || playerState.GetCurrentState() == CharacterStateType.Death)
        {
            inputHandler.Player.Disable();
            return;
        }
        else if (playerState.GetCurrentState() == CharacterStateType.Knockback)
        {
            if (!IsWaitForKnockBack)
            {
                CoroutineRunner.Instance.RunCoroutine(WaitForKnockBack());
            }
            return;
        }
        else
        {
            inputHandler.Player.Enable();
            if (CheckIfMoving())
                playerState.ChangeState(CharacterStateType.Running);
            else
                playerState.ChangeState(CharacterStateType.Idle);
        }   
    }

    private bool CheckIfMoving()
    {
        return rb.linearVelocity.sqrMagnitude > 0.05f;
    }

    private IEnumerator WaitForKnockBack()
    {
        IsWaitForKnockBack = true;
        inputHandler.Player.Disable();
        yield return new WaitForSeconds(0.7f);
        if (playerState.GetCurrentState() == CharacterStateType.Knockback)
        {
            playerState.ChangeState(CharacterStateType.Idle);
            IsWaitForKnockBack = false;
        }
    }
}
