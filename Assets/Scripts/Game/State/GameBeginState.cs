using UnityEngine;

public class GameBeginState : GameState
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
    }
    public override void Update() { }
    public override void Exit() { }
}
