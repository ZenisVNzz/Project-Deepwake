using UnityEngine;

public class GameStateMachine
{
    public SceneLoader _sceneLoader;
    public GameState CurrentState { get; private set; }

    public GameStateMachine(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void ChangeState<T>() where T : GameState, new()
    {
        CurrentState?.Exit();
        CurrentState = new T();
        CurrentState.Enter(this);
    }
}
