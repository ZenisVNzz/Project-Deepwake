using UnityEngine;

public enum EquipmentType { Special, Chestplate, Weapon, Ring, Necklace }

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class EquipmentData : ItemData
{
    public EquipmentType equipmentType;
    public int maxHealthBonus;
    public int maxStaminaBonus;
    public int attackPowerBonus;
    public int defenseBonus;
    public int speedBonus;
    public int criticalChanceBonus;
    public int criticalDamageBonus;
    public int vitBonus;
    public int strBonus;
    public int luckBonus;
}