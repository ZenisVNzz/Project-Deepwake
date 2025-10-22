using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Dictionary<string, IRuntimeUI> _uiDictionary = new();
    [SerializeField] private UIHealthBar healthBar;
    [SerializeField] private UIStaminaBar staminaBar;
    [SerializeField] private UIInventory inventoryUI;
    public void RegisterUI(string key, IRuntimeUI ui)
    {
        if (!_uiDictionary.ContainsKey(key))
            _uiDictionary.Add(key, ui);
    }

    public T GetUI<T>(string key) where T : class, IRuntimeUI
    {
        if (_uiDictionary.TryGetValue(key, out var ui))
            return ui as T;
        return null;
    }

    public void UpdateHealth(float current, float max)
    {
        healthBar.SetValue(current, max);
    }

    public void UpdateStamina(float current, float max)
    {
        staminaBar.SetValue(current, max);
    }

    public void ToggleInventory(bool show)
    {
        if (show)
            inventoryUI.Show();
        else
            inventoryUI.Hide();
    }

}