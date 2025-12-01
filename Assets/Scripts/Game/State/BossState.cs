using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

namespace Assets.Scripts.Game.State
{
	public class BossState: GameState
	{
        public override void Enter(GameStateMachine machine)
        {
            base.Enter(machine);
            GameObject triggerObject = EnvironmentSpawner.Instance.Spawn("Trigger", new Vector3(25f, -3f, 0f), false, true);
            Trigger trigger = triggerObject.GetComponent<Trigger>();

            if (GameController.Instance.CurrentLevel == 0)
            {
                Vector2 spawnPos = ShipController.Instance.transform.position + new Vector3(-2.5f, 10f, 0f);
                trigger.RegisterAction(() =>
                {
                    BossSpawner.Instance.SpawnBoss("GhostPirateShip", spawnPos);
                });
            }     
        }
        public override void Update() { }
        public override void Exit() { }
    }
}