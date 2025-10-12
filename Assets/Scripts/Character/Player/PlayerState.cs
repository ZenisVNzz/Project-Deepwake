using UnityEngine;

public class PlayerState : IState
{
    private CharacterStateType CurrentState;

    public void ChangeState(CharacterStateType newState)
    {
        CurrentState = newState;
    }
    public CharacterStateType GetCurrentState()
    {
        return CurrentState;
    }
}
