//using UnityEngine;
//using System;
//using System.Collections.Generic;

//[Serializable]
//public class PlayerData
//{

//    public const string CURRENCY_STATE_KEY = "CurrencyState";
//    public const string CHARACTER_STATE_KEY = "CharacterState";
//    public const string INVENTORY_STATE_KEY = "InventoryList";

//    public PlayerCurrencyState CurrencyState = new PlayerCurrencyState();

//    public int PlayerLevel = 1;
//    public float CurrentEXP = 0f;

//    public List<PlayerInventoryItem> Inventory = new List<PlayerInventoryItem>();

//    [Serializable]
//    public class PlayerInventoryItem
//    {
//        public string ItemId;
//        public int StackCount;
//    }
//}

using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public const string CHARACTER_STATE_KEY = "PlayerFullData";
    public string LastLoginTime { get; set; } = "N/A";
    public PlayerCurrencyState CurrencyWallet = new PlayerCurrencyState();

    public int PlayerLevel = 1;
    public float CurrentEXP = 0f;
}