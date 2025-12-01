using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    public static GameController Instance { get; private set; }

    public GameStateMachine gameStateMachine;
    private SceneLoader sceneLoader;

    public int CurrentLevel = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        sceneLoader = SceneLoader.Instance;
        gameStateMachine = new GameStateMachine(sceneLoader);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        OnStartGame();
    }

    private void OnStartGame()
    {
        gameStateMachine.ChangeState<GameBeginState>();
    }

    public void NextLevel()
    {
        CurrentLevel++;

        switch (CurrentLevel)
        {
            case 1:
                gameStateMachine.ChangeState<LevelOneState>();
                break;
            case 2:
                gameStateMachine.ChangeState<LevelTwoState>();
                break;
            default:
                gameStateMachine.ChangeState<LevelThreeState>();
                break;
        }
    }

    private void Update()
    {
        if (gameStateMachine.CurrentState != null)
        {
            gameStateMachine.CurrentState.Update();
        }             
    }
}
