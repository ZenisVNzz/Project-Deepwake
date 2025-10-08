using UnityEngine;
using System.Collections.Generic;

public class EquipmentManager : MonoBehaviour
{
    public Dictionary<EquipmentType, EquipmentData> equippedItems = new();

    public void Equip(EquipmentData newEquip)
    {
        EquipmentType slot = newEquip.equipmentType;

        if (equippedItems.ContainsKey(slot))
        {
            Debug.Log($"Replaced {equippedItems[slot].itemName} with {newEquip.itemName}");
            equippedItems[slot] = newEquip;
        }
        else
        {
            equippedItems.Add(slot, newEquip);
        }

        ApplyStats();
    }

    public void Unequip(EquipmentType slot)
    {
        if (equippedItems.ContainsKey(slot))
        {
            equippedItems.Remove(slot);
            ApplyStats();
        }
    }

    private void ApplyStats()
    {
    }

    //equipmentManager.Equip(item as EquipmentData);

    //playerInventory.AddItem(itemData);
}