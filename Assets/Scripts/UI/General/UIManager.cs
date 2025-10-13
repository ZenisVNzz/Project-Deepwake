using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Dictionary<string, IRuntimeUI> _uiDictionary = new();

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

    public void ShowUI(string key)
    {
        if (_uiDictionary.TryGetValue(key, out var ui))
            ui.Show();
    }

    public void HideUI(string key)
    {
        if (_uiDictionary.TryGetValue(key, out var ui))
            ui.Hide();
    }

    public void UpdateAllUI()
    {
        foreach (var ui in _uiDictionary.Values)
            ui.UpdateUI();
    }
}