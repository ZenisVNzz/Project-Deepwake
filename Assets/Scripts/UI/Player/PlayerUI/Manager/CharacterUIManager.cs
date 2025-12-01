using Mirror;
using System;
using UnityEngine;

public class CharacterUIManager : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject PopupCanvas;
    [SerializeField] private GameObject CharMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject mapUI;
    [SerializeField] private UIStatusBar statusBar;
    [SerializeField] private UIStats statsPanel;
    [SerializeField] private UIAttributesPanel attributesPanel;
    [SerializeField] private UIInventory inventoryPanel;
    [SerializeField] private UIEquipmentPanel equipmentPanel;
    [SerializeField] private UISkillTreePanel skillTreePanel;
    [SerializeField] private CurrencyUI currencyUI;

    private IPlayerRuntime player;

    private CharacterAttributes attributes = new CharacterAttributes();

    public void Init(IPlayerRuntime runtime)
    {
        player = runtime;

        if (statusBar != null)
        {
            statusBar.Initialize();
            statusBar.BindData(player);
            statusBar.Show();
        }

        if (statsPanel != null)
        {
            statsPanel.UpdateStats(player as PlayerRuntime);
        }

        if (attributesPanel != null)
        {
            player.OnLevelUp += GrantAttributePoints;
            SyncAttributesFromRuntime();
            attributesPanel.Bind(attributes);
            attributesPanel.OnAddPointRequested += OnAddPointRequested;
        }

        if (inventoryPanel != null)
        {
            inventoryPanel.SetData(player);
            inventoryPanel.Hide();
        }

        if (equipmentPanel != null)
        {
            equipmentPanel.BindData(player);
            player.PlayerEquipment.OnEquipmentChanged += OnEquipmentChanged;
        }

        if (skillTreePanel != null)
        {
        }

        if (currencyUI != null)
        {
            currencyUI.Bind(player);
        }

        UICanvas.transform.SetParent(null, false);
        UICanvas.SetActive(true);
        PopupCanvas.transform.SetParent(null, false);
        PopupCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        if (attributesPanel != null)
            attributesPanel.OnAddPointRequested -= OnAddPointRequested;
    }

    private void OnAddPointRequested(string key)
    {
        if (attributes.AvailablePoints <= 0) return;
        switch (key)
        {
            case "VIT": attributes.VIT++; break;
            case "DEF": attributes.DEF++; break;
            case "STR": attributes.STR++; break;
            case "LUCK": attributes.LUCK++; break;
            default: return;
        }
        attributes.AvailablePoints--;

        ApplyAttributesToRuntime();
    }

    public void GrantAttributePoints(int level)
    {
        if (level <= 100)
            attributes.AvailablePoints += Math.Max(0, 5);

        if (attributesPanel != null)
        {
            attributesPanel.Bind(attributes);
        }

        ApplyAttributesToRuntime();
    }

    private void ApplyAttributesToRuntime()
    {
        if (player is PlayerRuntime runtime)
        {
            runtime.ApplyAttributes(attributes);
            if (statsPanel != null)
            {
                statsPanel.UpdateStats(runtime);
            }

            if (statusBar != null)
            {
                statusBar.UpdateUI();
            }
        }
    }

    private void SyncAttributesFromRuntime()
    {
        if (player is CharacterRuntime runtime && runtime.RuntimeAttributes != null)
        {
            attributes = runtime.RuntimeAttributes;
        }
    }

    private void OnEquipmentChanged(Equipment equipment)
    {
        equipmentPanel.UpdateSlot(equipment);
        ApplyAttributesToRuntime();
    }

    public void ToggleCharacterMenu()
    {
        if (CharMenu == null) return;
        CharMenu.SetActive(!CharMenu.activeSelf);
    }

    public void ToggleOptionsMenu()
    {
        if (OptionsMenu == null) return;
        OptionsMenu.SetActive(!OptionsMenu.activeSelf);
    }

    public void ToggleMapUI()
    {
        if (mapUI == null) return;
        mapUI.SetActive(!mapUI.activeSelf);
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;
        if (inventoryPanel.gameObject.activeSelf) inventoryPanel.Hide(); else inventoryPanel.Show();
    }

    public void ToggleSkillTree()
    {
        if (skillTreePanel == null) return;
        skillTreePanel.gameObject.SetActive(!skillTreePanel.gameObject.activeSelf);
    }

    public void ToggleEquipment()
    {
        if (equipmentPanel == null) return;
        equipmentPanel.gameObject.SetActive(!equipmentPanel.gameObject.activeSelf);
    }

    public void ToggleUICanvas()
    {
        if (UICanvas == null) return;
        UICanvas.SetActive(!UICanvas.activeSelf);
    }    
}
