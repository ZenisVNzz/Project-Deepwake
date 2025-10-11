using UnityEngine;

public class EnemyState : IState
{
    private CharacterStateType CurrentState;

    public void ChangeState(CharacterStateType newState)
    {
        CurrentState = newState;
        Debug.LogWarning($"Change State To {newState}");
    }
    public CharacterStateType GetCurrentState()
    {
        return CurrentState;
    }
}
