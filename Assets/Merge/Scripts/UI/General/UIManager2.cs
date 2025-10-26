using System.Collections.Generic;
using UnityEngine;

public class UIManager2 : MonoBehaviour
{
    private Dictionary<string, IRuntimeUIService> _uiDictionary = new();

    [SerializeField] private UIInventory inventoryUI;
    public void RegisterUI(string key, IRuntimeUIService ui)
    {
        if (!_uiDictionary.ContainsKey(key))
            _uiDictionary.Add(key, ui);
    }

    public T GetUI<T>(string key) where T : class, IRuntimeUIService
    {
        if (_uiDictionary.TryGetValue(key, out var ui))
            return ui as T;
        return null;
    }

    public void ToggleInventory(bool show)
    {
        if (show)
            inventoryUI.Show();
        else
            inventoryUI.Hide();
    }

    [SerializeField] private MonoBehaviour healthBarComponent;
    [SerializeField] private MonoBehaviour staminaBarComponent;

    private IProgressBar healthBar;
    private IProgressBar staminaBar;

    private void Awake()
    {
        healthBar = healthBarComponent as IProgressBar;
        staminaBar = staminaBarComponent as IProgressBar;
    }

    public void Initialize(float maxHealth, float maxStamina)
    {
        healthBar?.Initialize(maxHealth);
        staminaBar?.Initialize(maxStamina);
    }

    public void UpdateHealth(float current)
    {
        healthBar?.SetValue(current);
    }

    public void UpdateStamina(float current)
    {
        staminaBar?.SetValue(current);
    }

}