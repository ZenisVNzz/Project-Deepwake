using UnityEngine;

[System.Serializable]
public class PlayerStateHandler : IStateHandler
{
    private IState playerState;
    private IMovable playerMovement;

    public PlayerStateHandler(IState state, IMovable playerMovement)
    {
        playerState = state;
        this.playerMovement = playerMovement;
    }

    public void UpdateState()
    {
        if (playerState.GetCurrentState() == CharacterStateType.Attacking)
            return;
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
