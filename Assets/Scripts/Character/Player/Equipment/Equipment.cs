using System;

[System.Serializable]
public class Equipment
{
    public EquipmentData Weapon;
    public EquipmentData Special;
    public EquipmentData Chestplate;
    public EquipmentData Ring;
    public EquipmentData Necklace;

    private IPlayerRuntime playerRuntime;

    public Action<Equipment> OnEquipmentChanged;

    public Equipment(IPlayerRuntime playerRuntime)
    {
        this.playerRuntime = playerRuntime;
    }

    public void Equip(ItemData item)
    {
        if (item.itemType != ItemType.Equipment)
            return;

        EquipmentData equip = item as EquipmentData;
        if (equip == null) return;

        switch (equip.equipmentType)
        {
            case EquipmentType.Weapon:
                if (Weapon != null)
                {
                    playerRuntime.PlayerInventory.AddItem(Weapon, 1);
                }

                Weapon = equip;
                break;

            case EquipmentType.Special:
                if (Special != null)
                {
                    playerRuntime.PlayerInventory.AddItem(Special, 1);
                }

                Special = equip;
                break;
            case EquipmentType.Chestplate:
                if (Chestplate != null)
                {
                    playerRuntime.PlayerInventory.AddItem(Chestplate, 1);
                }

                Chestplate = equip;
                break;

            case EquipmentType.Ring:
                if (Ring != null)
                {
                    playerRuntime.PlayerInventory.AddItem(Ring, 1);
                }

                Ring = equip;
                break;
            case EquipmentType.Necklace:
                if (Necklace != null)
                {
                    playerRuntime.PlayerInventory.AddItem(Necklace, 1);
                }

                Necklace = equip;
                break;
        }

        UpdateBonusStats();
    }

    public void Unequip(ItemData item)
    {
        if (item.itemType != ItemType.Equipment)
            return;
        EquipmentData equip = item as EquipmentData;
        if (equip == null) return;

        switch (equip.equipmentType)
        {
            case EquipmentType.Weapon:
                Weapon = null;
                break;
            case EquipmentType.Special:
                Special = null;
                break;
            case EquipmentType.Chestplate:
                Chestplate = null;
                break;
            case EquipmentType.Ring:
                Ring = null;
                break;
            case EquipmentType.Necklace:
                Necklace = null;
                break;
        }

        UpdateBonusStats();
    }

    private void UpdateBonusStats()
    {
        float bonusAttack = (Weapon?.attackPowerBonus ?? 0) + (Special?.attackPowerBonus ?? 0) + (Chestplate?.attackPowerBonus ?? 0) + (Ring?.attackPowerBonus ?? 0) + (Necklace?.attackPowerBonus ?? 0);
        float bonusDefense = (Weapon?.defenseBonus ?? 0) + (Special?.defenseBonus ?? 0) + (Chestplate?.defenseBonus ?? 0) + (Ring?.defenseBonus ?? 0) + (Necklace?.defenseBonus ?? 0);
        float bonusMaxHealth = (Weapon?.maxHealthBonus ?? 0) + (Special?.maxHealthBonus ?? 0) + (Chestplate?.maxHealthBonus ?? 0) + (Ring?.maxHealthBonus ?? 0) + (Necklace?.maxHealthBonus ?? 0);
        float bonusSpeed = (Weapon?.speedBonus ?? 0) + (Special?.speedBonus ?? 0) + (Chestplate?.speedBonus ?? 0) + (Ring?.speedBonus ?? 0) + (Necklace?.speedBonus ?? 0);
        float bonusStamina = (Weapon?.maxStaminaBonus ?? 0) + (Special?.maxStaminaBonus ?? 0) + (Chestplate?.maxStaminaBonus ?? 0) + (Ring?.maxStaminaBonus ?? 0) + (Necklace?.maxStaminaBonus ?? 0);
        float bonusCritChance = (Weapon?.criticalChanceBonus ?? 0) + (Special?.criticalChanceBonus ?? 0) + (Chestplate?.criticalChanceBonus ?? 0) + (Ring?.criticalChanceBonus ?? 0) + (Necklace?.criticalChanceBonus ?? 0);
        float bonusCritDmg = (Weapon?.criticalDamageBonus ?? 0) + (Special?.criticalDamageBonus ?? 0) + (Chestplate?.criticalDamageBonus ?? 0) + (Ring?.criticalDamageBonus ?? 0) + (Necklace?.criticalDamageBonus ?? 0);

        playerRuntime.ApplyBonusStat(BonusStat.AttackPower, bonusAttack);
        playerRuntime.ApplyBonusStat(BonusStat.Defense, bonusDefense);
        playerRuntime.ApplyBonusStat(BonusStat.Health, bonusMaxHealth);
        playerRuntime.ApplyBonusStat(BonusStat.Speed, bonusSpeed);
        playerRuntime.ApplyBonusStat(BonusStat.Stamina, bonusStamina);
        playerRuntime.ApplyBonusStat(BonusStat.CriticalChance, bonusCritChance);
        playerRuntime.ApplyBonusStat(BonusStat.CriticalDmg, bonusCritDmg);
        OnEquipmentChanged?.Invoke(this);
    }
}