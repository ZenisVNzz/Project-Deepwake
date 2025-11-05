using UnityEngine;
using UnityEngine.Localization;

public enum ItemType { Consumable, Material, Equipment }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemId;
    public LocalizedString itemName;
    public Sprite icon;
    public ItemType itemType;
    public int maxStack = 99;
}