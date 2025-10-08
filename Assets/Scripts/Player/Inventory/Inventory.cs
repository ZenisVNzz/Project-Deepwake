using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public int maxSlots = 20;

    public void AddItem(ItemData newItem, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == newItem && slot.quantity < newItem.maxStack)
            {
                slot.quantity += amount;
                return;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.item = newItem;
                slot.quantity = amount;
                return;
            }
        }

        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot { item = newItem, quantity = amount });
        }
        else
        {
            Debug.LogWarning("Inventory full!");
        }
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.quantity -= amount;
                if (slot.quantity <= 0)
                    slot.Clear();
                return;
            }
        }
    }
}