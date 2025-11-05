using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentPanel : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image specialIcon;
    [SerializeField] private Image chestplateIcon;
    [SerializeField] private Image ringIcon;
    [SerializeField] private Image necklaceIcon;

    private IPlayerRuntime playerRuntime;

    public void BindData(IPlayerRuntime runtime)
    {
        playerRuntime = runtime;
    }

    public void UpdateSlot(Equipment equipment)
    {
        if (equipment.Weapon != null)
        {
            weaponIcon.sprite = equipment.Weapon.icon;
            weaponIcon.color = Color.white;

            UIEquipmentSlot weaponSlot = weaponIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            weaponSlot.SetEquipmentData(equipment.Weapon);
        }
        else
        {
            weaponIcon.sprite = null;
            weaponIcon.color = Color.clear;

            UIEquipmentSlot weaponSlot = weaponIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            weaponSlot.SetEquipmentData(null);
        }

        if (equipment.Special != null)
        {
            specialIcon.sprite = equipment.Special.icon;
            specialIcon.color = Color.white;

            UIEquipmentSlot specialSlot = specialIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            specialSlot.SetEquipmentData(equipment.Special);
        }
        else
        {
            specialIcon.sprite = null;
            specialIcon.color = Color.clear;

            UIEquipmentSlot specialSlot = specialIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            specialSlot.SetEquipmentData(null);
        }

        if (equipment.Chestplate != null)
        {
            chestplateIcon.sprite = equipment.Chestplate.icon;
            chestplateIcon.color = Color.white;

            UIEquipmentSlot chestplateSlot = chestplateIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            chestplateSlot.SetEquipmentData(equipment.Chestplate);
        }
        else
        {
            chestplateIcon.sprite = null;
            chestplateIcon.color = Color.clear;

            UIEquipmentSlot chestplateSlot = chestplateIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            chestplateSlot.SetEquipmentData(null);
        }

        if (equipment.Ring != null)
        {
            ringIcon.sprite = equipment.Ring.icon;
            ringIcon.color = Color.white;

            UIEquipmentSlot ringSlot = ringIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            ringSlot.SetEquipmentData(equipment.Ring);
        }
        else
        {
            ringIcon.sprite = null;
            ringIcon.color = Color.clear;

            UIEquipmentSlot ringSlot = ringIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            ringSlot.SetEquipmentData(null);
        }

        if (equipment.Necklace != null)
        {
            necklaceIcon.sprite = equipment.Necklace.icon;
            necklaceIcon.color = Color.white;

            UIEquipmentSlot necklaceSlot = necklaceIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            necklaceSlot.SetEquipmentData(equipment.Necklace);
        }
        else
        {
            necklaceIcon.sprite = null;
            necklaceIcon.color = Color.clear;

            UIEquipmentSlot necklaceSlot = necklaceIcon.transform.GetComponentInParent<UIEquipmentSlot>();
            necklaceSlot.SetEquipmentData(null);
        }
    }
}
