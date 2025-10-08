using UnityEngine;

public enum EquipmentType { Head, Body, Weapon, Accessory }

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class EquipmentData : ItemData
{
    public EquipmentType equipmentType;
    public int attackBonus;
    public int defenseBonus;
    public int healthBonus;
}