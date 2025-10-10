using UnityEngine;

[System.Serializable]
public class PlayerStateHandler : IStateHandler
{
    private IState playerState;
    private Rigidbody2D rb;
    private InputSystem_Actions inputHandler;

    public PlayerStateHandler(IState state, Rigidbody2D rigidbody2D, InputSystem_Actions inputHandler)
    {
        playerState = state;
        rb = rigidbody2D;
        this.inputHandler = inputHandler;
    }

    public void UpdateState()
    {
        if (playerState.GetCurrentState() == CharacterStateType.Attacking)
        {
            inputHandler.Player.Disable();
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
        return rb.linearVelocity.sqrMagnitude > 0.01f;
    }
}
