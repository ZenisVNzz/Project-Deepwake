using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ShopCategories
{
    Weapon,
    Chestplate,
    Ring,
    Necklace,
    Special,
    Other
}

[CreateAssetMenu(fileName = "NewCategory", menuName = "Data/Shop/Category")]
public class Category : ScriptableObject
{
    public List<ItemCategory> itemCategories = new();
}

[Serializable]
public class ItemCategory
{
    public ItemData ItemData;
    public float rate;
    public int stock;
    public int price;
    public CurrencyType currencyType;
}
