using UnityEngine;

public class GameController : MonoBehaviour
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

        sceneLoader = new SceneLoader();
        gameStateMachine = new GameStateMachine(sceneLoader);
    }

    private void Start()
    {
        gameStateMachine.ChangeState<GameBeginState>();
    }

    private void Update()
    {
        gameStateMachine.CurrentState.Update();
    }
}
