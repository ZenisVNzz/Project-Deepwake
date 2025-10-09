using UnityEngine;

public enum CharacterStateType
{
    Idle,
    Running,
    Jumping,
    Falling,
    Knockback,
    Attacking
}

public interface IState
{
    void ChangeState(CharacterStateType newState);
    CharacterStateType GetCurrentState();
}
