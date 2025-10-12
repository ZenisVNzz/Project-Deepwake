using UnityEngine;

public enum CharacterStateType
{
    Idle,
    Running,
    Jumping,
    Falling,
    Knockback,
    Attacking,
    Death
}

public interface IState
{
    void ChangeState(CharacterStateType newState);
    CharacterStateType GetCurrentState();
}
