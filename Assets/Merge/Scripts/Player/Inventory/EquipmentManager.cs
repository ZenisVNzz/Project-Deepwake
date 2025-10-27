using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    public event Action<EquipmentType, EquipmentData> OnEquipmentChanged;

    private Dictionary<EquipmentType, EquipmentData> equippedItems = new();
    private CharacterRuntime _characterStats;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _characterStats = FindAnyObjectByType<CharacterRuntime>();
    }

    public void Equip(EquipmentData newEquip)
    {
        EquipmentType slot = newEquip.equipmentType;

        if (equippedItems.TryGetValue(slot, out var oldEquip))
        {
            RemoveStats(oldEquip);
            Debug.Log($"Replaced {oldEquip.itemName} with {newEquip.itemName}");
        }

        equippedItems[slot] = newEquip;
        ApplyStats(newEquip);

        OnEquipmentChanged?.Invoke(slot, newEquip);
    }

    public void Unequip(EquipmentType slot)
    {
        if (!equippedItems.ContainsKey(slot)) return;

        var oldEquip = equippedItems[slot];
        RemoveStats(oldEquip);
        equippedItems.Remove(slot);

        OnEquipmentChanged?.Invoke(slot, null);
    }

    private void ApplyStats(EquipmentData equip)
    {
        if (_characterStats == null) return;

        //_characterStats.bonusMaxHealth += equip.maxHealthBonus;
        //_characterStats.bonusStamina += equip.maxStaminaBonus;
        //_characterStats.bonusAttackPower += equip.attackPowerBonus;
        //_characterStats.bonusDefense += equip.defenseBonus;
        //_characterStats.bonusSpeed += equip.speedBonus;
        //_characterStats.bonusCriticalChance += equip.criticalChanceBonus;
        //_characterStats.bonusCriticalDamage += equip.criticalDamageBonus;
        //_characterStats.bonusVitality += equip.vitBonus;
        //_characterStats.bonusStrength += equip.strBonus;
        //_characterStats.bonusLuck += equip.luckBonus;
    }

    private void RemoveStats(EquipmentData equip)
    {
        if (_characterStats == null) return;

        //_characterStats.bonusMaxHealth -= equip.maxHealthBonus;
        //_characterStats.bonusStamina -= equip.maxStaminaBonus;
        //_characterStats.bonusAttackPower -= equip.attackPowerBonus;
        //_characterStats.bonusDefense -= equip.defenseBonus;
        //_characterStats.bonusSpeed -= equip.speedBonus;
        //_characterStats.bonusCriticalChance -= equip.criticalChanceBonus;
        //_characterStats.bonusCriticalDamage -= equip.criticalDamageBonus;
        //_characterStats.bonusVitality -= equip.vitBonus;
        //_characterStats.bonusStrength -= equip.strBonus;
        //_characterStats.bonusLuck -= equip.luckBonus;
    }

    public EquipmentData GetEquipped(EquipmentType slot)
    {
        return equippedItems.TryGetValue(slot, out var data) ? data : null;
    }
}

