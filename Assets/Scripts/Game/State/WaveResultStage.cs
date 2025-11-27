using UnityEngine;

public class WaveResultStage : GameState
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

        WaveController waveController = GameObject.FindAnyObjectByType<WaveController>();
        if (waveController == null)
        {
            Debug.LogError("[BattleState] WaveController not found in the scene.");
            return;
        }

        waveController.IncreaseDifficulty();
        ChestSpawner chestSpawner = new ChestSpawner();

        if (waveController.CurrentWave >= 0)
        {
            chestSpawner.SpawnChestOnShip(1);
        }
        else if (waveController.CurrentWave >= 3)
        {
            chestSpawner.SpawnChestOnShip(2);
        }
        else if (waveController.CurrentWave >= 6)
        {
            chestSpawner.SpawnChestOnShip(3);
        }
        else if (waveController.CurrentWave >= 9)
        {
            chestSpawner.SpawnChestOnShip(4);
        }

        chestSpawner.IncreaseChestRate(1);
        shipWheel.SetActive();
    }
    public override void Update() { }
    public override void Exit() { }
}
