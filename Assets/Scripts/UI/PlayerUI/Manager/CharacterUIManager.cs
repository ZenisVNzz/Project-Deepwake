using System;
using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject CharMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private UIStatusBar statusBar;
    [SerializeField] private UIStats statsPanel;
    [SerializeField] private UIAttributesPanel attributesPanel;
    [SerializeField] private UIInventory inventoryPanel;
    [SerializeField] private UIEquipmentPanel equipmentPanel;
    [SerializeField] private UISkillTreePanel skillTreePanel;

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

            SyncAttributesFromRuntime();
            attributesPanel.Bind(attributes);
            attributesPanel.OnAddPointRequested += OnAddPointRequested;
        }

        if (inventoryPanel != null)
        {
            inventoryPanel.SetData(player.PlayerInventory);
            inventoryPanel.Hide();
        }

        if (skillTreePanel != null)
        {
        }
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

    public void GrantAttributePoints(int amount)
    {
        attributes.AvailablePoints += Math.Max(0, amount);
        if (attributesPanel != null)
        {
            attributesPanel.Bind(attributes);
        }
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
}
