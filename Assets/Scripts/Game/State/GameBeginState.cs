using UnityEngine;
using UnityEngine.Localization;

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

        UIManager.Instance.GetPopupService().Create("100003", "GAME_BEGIN", new LocalizedString("Level", "LevelOne"), null, null);
        UIManager.Instance.GetPopupService().Destroy("GAME_BEGIN", 4.5f);
        ResourceManager.Instance.ReleaseAssetReferences("TitleScene");
    }
    public override void Update() { }
    public override void Exit() { }
}
