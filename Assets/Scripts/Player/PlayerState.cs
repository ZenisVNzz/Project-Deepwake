using UnityEngine;

public enum CharacterStateType
{
    Idle,
    Running,
    Jumping,
    Falling,
    Attacking
}

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
