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

        shipWheel.SetActive();

        EnvironmentSpawner environmentSpawner = EnvironmentSpawner.Instance;
        GameObject raftGo = environmentSpawner.Spawn("WoodRaft", new Vector3(20f, -6f, 0f), true, false);
        ObjectCleaner.Instance.ObjectToClean.Add(raftGo);

        if (UIManager.Instance.RuntimeUIServiceRegistry.TryGet<ShopUI>(out ShopUI shopUI))
        {
            shopUI.Shop.InitCategory();
        }
    }
    public override void Update() { }
    public override void Exit() { }
}
