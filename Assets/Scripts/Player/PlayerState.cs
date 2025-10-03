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
    public CharacterStateType CurrentState { get; private set; }

    public void ChangeState(CharacterStateType newState)
    {
        CurrentState = newState;
    }
}
