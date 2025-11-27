using UnityEngine;
using UnityEngine.Localization;

public enum ItemType { Consumable, Material, Equipment }
public enum ItemTier { Common, Rare, Epic, Legendary }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemId;
    public LocalizedString itemName;
    public LocalizedString itemDescription;
    public ItemTier itemTier;
    public Sprite icon;
    public ItemType itemType;
    public int maxStack = 99;
}