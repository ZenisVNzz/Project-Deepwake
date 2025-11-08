using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Data/Database/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    private Dictionary<string, ItemData> lookup;

    public void Initialize()
    {
        lookup = new Dictionary<string, ItemData>();
        foreach (var item in items)
            lookup[item.itemId] = item;
    }

    public ItemData Get(string id)
    {
        if (lookup == null) Initialize();
        lookup.TryGetValue(id, out var result);
        return result;
    }
}
