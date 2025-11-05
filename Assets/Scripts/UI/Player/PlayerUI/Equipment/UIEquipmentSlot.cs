using UnityEngine;

public class UIEquipmentSlot : MonoBehaviour
{
    private EquipmentData equipmentData;
    public EquipmentData EquipmentData => equipmentData;

    [SerializeField] private GameObject blankIcon;
    [SerializeField] private GameObject itemIcon;

    public void SetEquipmentData(EquipmentData data)
    {
        equipmentData = data;
        if (data != null)
        {
            blankIcon.SetActive(false);
            itemIcon.SetActive(true);
        }
        else
        {
            blankIcon.SetActive(true);
            itemIcon.SetActive(false);
        }
    }
}
