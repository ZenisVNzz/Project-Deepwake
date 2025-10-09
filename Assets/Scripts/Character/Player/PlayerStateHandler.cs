using UnityEngine;

[System.Serializable]
public class PlayerStateHandler : IStateHandler
{
    private IState playerState;
    private IMovable playerMovement;
    private InputSystem_Actions inputHandler;

    public PlayerStateHandler(IState state, IMovable playerMovement, InputSystem_Actions inputHandler)
    {
        playerState = state;
        this.playerMovement = playerMovement;
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
        Vector2 vec = playerMovement.GetDir();

        if (vec.sqrMagnitude > 0.01f)
            return true;
        else
            return false;
    }
}
