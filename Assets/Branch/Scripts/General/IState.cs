using UnityEngine;

public interface IState
{
    void ChangeState(CharacterStateType newState);
}
