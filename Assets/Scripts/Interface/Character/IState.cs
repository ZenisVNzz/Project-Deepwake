using UnityEngine;

public enum CharacterStateType
{
    Awake,
    Idle,
    Running,
    Jumping,
    Falling,
    Knockback,
    Attacking,
    Death,
    Revive
}

public interface IState
{
    void ChangeState(CharacterStateType newState);
    CharacterStateType GetCurrentState();
}
