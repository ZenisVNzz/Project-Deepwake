using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentPanel : MonoBehaviour
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image helmetIcon;
    [SerializeField] private Image chestplateIcon;
    [SerializeField] private Image ringIcon;
    [SerializeField] private Image necklaceIcon;

    private void OnEnable()
    {
        EquipmentManager.Instance.OnEquipmentChanged += UpdateSlot;
    }

    private void OnDisable()
    {
        EquipmentManager.Instance.OnEquipmentChanged -= UpdateSlot;
    }

    private void UpdateSlot(EquipmentType type, EquipmentData data)
    {
        Image target = type switch
        {
            EquipmentType.Weapon => weaponIcon,
            EquipmentType.Helmet => helmetIcon,
            EquipmentType.Chestplate => chestplateIcon,
            EquipmentType.Ring => ringIcon,
            EquipmentType.Necklace => necklaceIcon,
            _ => null
        };

        if (target != null)
            target.sprite = data != null ? data.icon : null;
    }
}
