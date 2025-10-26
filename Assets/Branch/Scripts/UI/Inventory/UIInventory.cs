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

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;

    private readonly List<UIInventorySlot> activeSlots = new();
    private readonly Queue<UIInventorySlot> pool = new();

    public void RefreshUI(List<InventorySlot> dataSlots)
    {
        foreach (var s in activeSlots)
        {
            s.gameObject.SetActive(false);
            pool.Enqueue(s);
        }
        activeSlots.Clear();

        foreach (var slotData in dataSlots)
        {
            UIInventorySlot slotUI;
            if (pool.Count > 0)
                slotUI = pool.Dequeue();
            else
                slotUI = Instantiate(slotPrefab, slotParent).GetComponent<UIInventorySlot>();

            slotUI.gameObject.SetActive(true);
            slotUI.Bind(slotData);
            activeSlots.Add(slotUI);
        }
    }
}