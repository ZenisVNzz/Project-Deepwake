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
}
