using UnityEngine;

public class PlayerStateHandler
{
    private PlayerState playerState;
    private IMovable playerMovement;

    public PlayerStateHandler(PlayerState state, IMovable playerMovement)
    {
        playerState = state;
        this.playerMovement = playerMovement;
    }

    public void UpdateState()
    {
        if (CheckIfMoving())
            playerState.ChangeState(CharacterStateType.Running);
        else
            playerState.ChangeState(CharacterStateType.Idle);
    }

    private bool CheckIfMoving()
    {
        Vector2 vec = playerMovement.GetDir();

        if (vec.magnitude > 0)
            return true;
        else
            return false;
    }
}
