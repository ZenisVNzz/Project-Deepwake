using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public int maxSlots = 12;

    public Action<List<InventorySlot>> OnInventoryChanged;

    public Inventory(int slotCount = 12)
    {
        maxSlots = slotCount;
    }

    public bool AddItem(ItemData newItem, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == newItem && slot.quantity < newItem.maxStack)
            {
                slot.quantity += amount;
                OnInventoryChanged?.Invoke(slots);
                Debug.Log($"Added item {newItem.itemName} to existing stack.");
                return true;
            }
        }

        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.item = newItem;
                slot.quantity = amount;
                OnInventoryChanged?.Invoke(slots);
                Debug.Log($"Added item {newItem.itemName} to new slot.");
                return true;
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
        return false;
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.quantity -= amount;
                if (slot.quantity <= 0)
                    slot.Clear();
                OnInventoryChanged?.Invoke(slots);
                Debug.Log($"Removed item {item.itemName} from inventory.");
                return true;
            }
        }
        return false;
    }
}