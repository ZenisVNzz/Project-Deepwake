using Mirror;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    public static GameController Instance { get; private set; }

    public GameStateMachine gameStateMachine;
    private SceneLoader sceneLoader;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
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
        gameStateMachine.ChangeState<GameBeginState>();
    }

    private void Update()
    {
        gameStateMachine.CurrentState.Update();
    }
}
