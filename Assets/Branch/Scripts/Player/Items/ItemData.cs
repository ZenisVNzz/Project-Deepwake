using UnityEngine;

public enum ItemType { Consumable, Material, Equipment }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemId;
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public int maxStack = 99;
}