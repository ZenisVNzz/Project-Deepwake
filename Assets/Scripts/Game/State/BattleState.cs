using UnityEngine;

public class BattleState : GameState
{
    public override void Enter(GameStateMachine machine)
    {
        base.Enter(machine);
        ShipWheel shipWheel = GameObject.FindAnyObjectByType<ShipWheel>();
        if (shipWheel == null)
        {
            Debug.LogError("[GameController] ShipWheel not found in the scene.");
            return;
        }

        shipWheel.SetInactive();

        WaveController waveController = GameObject.FindAnyObjectByType<WaveController>();
        if (waveController == null)
        {
            Debug.LogError("[BattleState] WaveController not found in the scene.");
            return;
        }

        waveController.SpawnNextWave();
    }
    public override void Update() { }
    public override void Exit() { }
}
