[System.Serializable]
public class Equipment
{
    public EquipmentData Weapon;
    public EquipmentData Special;
    public EquipmentData Chestplate;
    public EquipmentData Ring;
    public EquipmentData Necklace;

    public void Equip(ItemData item)
    {
        if (item.itemType != ItemType.Equipment)
            return;

        EquipmentData equip = item as EquipmentData;
        if (equip == null) return;

        switch (equip.equipmentType)
        {
            case EquipmentType.Weapon:
                Weapon = equip;
                break;

            case EquipmentType.Special:
                Special = equip;
                break;
            case EquipmentType.Chestplate:
                Chestplate = equip;
                break;

            case EquipmentType.Ring:
                Ring = equip;
                break;
            case EquipmentType.Necklace:
                Necklace = equip;
                break;
        }
    }
}