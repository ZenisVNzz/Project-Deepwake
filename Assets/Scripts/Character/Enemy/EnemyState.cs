using UnityEngine;

public class EnemyState : IState
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
