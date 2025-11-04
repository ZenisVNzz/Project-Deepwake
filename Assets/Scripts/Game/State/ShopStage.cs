using UnityEngine;

public class ShopStage : GameState
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

        EnvironmentSpawner environmentSpawner = EnvironmentSpawner.Instance;
        environmentSpawner.Spawn("WoodRaft", new Vector3(20f, -6f, 0f), true);
    }
    public override void Update() { }
    public override void Exit() { }
}
