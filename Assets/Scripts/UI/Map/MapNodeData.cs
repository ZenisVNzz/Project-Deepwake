using Assets.Scripts.Game.State;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapNodeData
{
    public Vector2Int gridPos;
    public NodeType nodeType;

    public void OnSelect()
    {
        GameController.Instance.CurrentNode++;
        Debug.Log($"Selected node {nodeType.NodeTypes}");
        if (nodeType.NodeTypes == NodeTypes.Monster)
        {
            GameController.Instance.gameStateMachine.ChangeState<BattleState>();
        }
        else if (nodeType.NodeTypes == NodeTypes.Treasure)
        {
            GameController.Instance.gameStateMachine.ChangeState<TreasureStage>();
        }
        else if (nodeType.NodeTypes == NodeTypes.Shop)
        {
            GameController.Instance.gameStateMachine.ChangeState<ShopStage>();
        }
        else if (nodeType.NodeTypes == NodeTypes.Boss)
        {
            GameController.Instance.gameStateMachine.ChangeState<BossState>();
        }
    }
}
