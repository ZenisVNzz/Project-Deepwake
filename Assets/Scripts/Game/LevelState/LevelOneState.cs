using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class LevelOneState : GameState
{
    public override void Enter(GameStateMachine machine)
    {
        base.Enter(machine);
        GameObject triggerObject = EnvironmentSpawner.Instance.Spawn("Trigger", new Vector3(25f, -3f, 0f), false, true);
        Trigger trigger = triggerObject.GetComponent<Trigger>();

        trigger.RegisterAction(() =>
        {
            UIManager.Instance.GetPopupService().Create("100003", "GAME_BEGIN", new LocalizedString("Level", "LevelTwo"), null, null);
            UIManager.Instance.GetPopupService().Destroy("GAME_BEGIN", 4.5f);
            CoroutineRunner.Instance.StartCoroutine(ChnageSeaColor());
            CoroutineRunner.Instance.StartCoroutine(GiveReward());
        });
    }
    public override void Update() { }
    public override void Exit() { }

    private IEnumerator GiveReward()
    {
        yield return new WaitForSeconds(5.2f);
        GameController.Instance.gameStateMachine.ChangeState<TreasureStage>();
    }

    private IEnumerator ChnageSeaColor()
    {
        yield return new WaitForSeconds(2f);
        SeaModify.Instance.ChangeSeaColor(Color.green);
        ObjectCleaner.Instance.Clean();
    }
}
