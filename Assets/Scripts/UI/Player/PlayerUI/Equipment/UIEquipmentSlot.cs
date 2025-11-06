using System;
using UnityEngine;

public class UIEquipmentSlot : MonoBehaviour
{
    [SerializeField] private EquipmentData equipmentData;
    public EquipmentData EquipmentData => equipmentData;

    [SerializeField] private GameObject blankIcon;
    [SerializeField] private GameObject itemIcon;

    public Action<bool> OnEquipWeapon;

    public void SetEquipmentData(EquipmentData data)
    {
        equipmentData = data;
        if (data != null)
        {
            blankIcon.SetActive(false);
            itemIcon.SetActive(true);
            OnEquipWeapon?.Invoke(true);
        }
        else
        {
            blankIcon.SetActive(true);
            itemIcon.SetActive(false);
            OnEquipWeapon?.Invoke(false);
        }
    }
}
