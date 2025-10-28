using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour, IRuntimeUIService
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private int slotCount;

    private Inventory playerInventory;

    private readonly List<UIInventorySlot> activeSlots = new();
    private readonly Queue<UIInventorySlot> pool = new();

    public void SetData(Inventory playerInventory)
    {
        this.playerInventory = playerInventory;
        slotCount = playerInventory.maxSlots;
        Initialize();
    }

    public void Initialize()
    {
        for (int i = 0; i < slotCount; i++)
        {
            UIInventorySlot slotUI = Instantiate(slotPrefab, slotParent).GetComponent<UIInventorySlot>();
            InventorySlot slot = new InventorySlot();
            slotUI.Bind(slot);
            playerInventory.slots.Add(slot);
            activeSlots.Add(slotUI);
        }
    }

    public void Show() => panel.SetActive(true);
    public void Hide() => panel.SetActive(false);
    public void UpdateUI() { }
    public void BindData(IPlayerRuntime data) { }

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