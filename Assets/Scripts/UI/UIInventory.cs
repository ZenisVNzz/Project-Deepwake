using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour, IRuntimeUI
{
    public GameObject panel;
    public void Initialize() { Hide(); }
    public void Show() => panel.SetActive(true);
    public void Hide() => panel.SetActive(false);
    public void UpdateUI()
    {

    }
    public void BindData(object data)
    {

    }

    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform contentParent;

    private List<GameObject> slots = new List<GameObject>();

    public void Refresh(List<ItemData> items)
    {
        foreach (var slot in slots) Destroy(slot);
        slots.Clear();

        foreach (var item in items)
        {
            var slot = Instantiate(itemSlotPrefab, contentParent);
            slot.GetComponentInChildren<Text>().text = item.itemName;
            slots.Add(slot);
        }
    }
}