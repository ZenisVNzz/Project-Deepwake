using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerCurrencyState
{

    public Dictionary<string, int> Balances { get; set; } = new Dictionary<string, int>();

    public PlayerCurrencyState(CurrencyWallet wallet)
    {
        Balances.Clear();

        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            Balances[type.ToString()] = wallet.Get(type);
        }
    }

    public PlayerCurrencyState() { }
}
