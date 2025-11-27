using UnityEngine;

public class PlayerCheat : MonoBehaviour
{
    [ContextMenu("Add 5 Attributes point to player")]
    private void AddAttributePoint()
    {
        CharacterUIManager characterUIManager = GetComponent<CharacterUIManager>();
        characterUIManager.GrantAttributePoints(1);
    }

    [ContextMenu("Add 500 Exp to player")]
    private void AddExp()
    {
        PlayerRuntime playerRuntime = GetComponent<PlayerRuntime>();
        playerRuntime.GainExp(500);
    }

    [ContextMenu("Add 500 Gold to player")]
    private void AddMoney()
    {
        PlayerRuntime playerRuntime = GetComponent<PlayerRuntime>();
        CurrencyWallet currencyWallet = playerRuntime.CurrencyWallet;
        currencyWallet.Add(CurrencyType.Gold, 500);
    }

    [ContextMenu("Deal 50 damage to self")]
    private void DealDamageToSelf()
    {
        PlayerRuntime playerRuntime = GetComponent<PlayerRuntime>();
        playerRuntime.TakeDamage(50, Vector3.zero);
    }

    [ContextMenu("Clear enemy")]
    private void ClearEnemy()
    {
        WaveController waveController = FindAnyObjectByType<WaveController>();
        waveController.enemySpawner.ClearAllEnemies();
    }

    [ContextMenu("Go to combat node")]
    private void GoToCombatNode()
    {
        GameController.Instance.gameStateMachine.ChangeState<BattleState>();
    }

    [ContextMenu("Go to Treasure node")]
    private void GotoTreasureNode()
    {
        GameController.Instance.gameStateMachine.ChangeState<TreasureStage>();
    }

    [ContextMenu("Go to Shop node")]
    private void GotoShopNode()
    {
        GameController.Instance.gameStateMachine.ChangeState<ShopStage>();
    }
}
