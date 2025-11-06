using System;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
    Gold
}

public class CurrencyWallet
{
    [Serializable]
    public class CurrencyAmount
    {
        public CurrencyType type;
        public int amount;
    }

    [SerializeField]
    private List<CurrencyAmount> initialBalances = new List<CurrencyAmount>
    {
        new CurrencyAmount { type = CurrencyType.Gold, amount =0 }
    };

    private readonly Dictionary<CurrencyType, int> _balances = new Dictionary<CurrencyType, int>();

    public event Action<CurrencyType, int> OnCurrencyChanged;

    public CurrencyWallet()
    {
        foreach (var entry in initialBalances)
        {
            OnCurrencyChanged?.Invoke(entry.type, _balances[entry.type]);
        }
    }

    public int Get(CurrencyType type)
    {
        return _balances.TryGetValue(type, out int value) ? value : 0;
    }

    public void Set(CurrencyType type, int amount, bool save = true)
    {
        int clamped = Mathf.Max(0, amount);
        _balances[type] = clamped;
        OnCurrencyChanged?.Invoke(type, clamped);
    }

    public bool TrySpend(CurrencyType type, int amount, bool save = true)
    {
        if (amount <= 0) return true;
        int current = Get(type);
        if (current < amount) return false;
        current -= amount;
        _balances[type] = current;
        OnCurrencyChanged?.Invoke(type, current);
        return true;
    }

    public void Add(CurrencyType type, int amount, bool save = true)
    {
        if (amount == 0) return;
        int current = Get(type) + amount;
        if (current < 0) current = 0;
        _balances[type] = current;
        OnCurrencyChanged?.Invoke(type, current);
    }

    public int Gold => Get(CurrencyType.Gold);
}
