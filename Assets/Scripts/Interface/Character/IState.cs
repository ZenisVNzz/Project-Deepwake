using UnityEngine;

public enum CharacterStateType
{
    Idle,
    Running,
    Jumping,
    Falling,
    Attacking
}

public interface IState
{
    void ChangeState(CharacterStateType newState);
    CharacterStateType GetCurrentState();
}
