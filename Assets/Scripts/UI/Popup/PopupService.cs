using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class PopupService : IPopupService
{
    private Dictionary<string, GameObject> _popupsPrefab = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _activePopups = new Dictionary<string, GameObject>();
    private GameObject canvas;

    public PopupService()
    {
        PopupContainer popup = ResourceManager.Instance.GetAsset<PopupContainer>("PopupContainer");
        foreach (var entry in popup.Popups)
        {
            _popupsPrefab[entry.ID] = entry.Prefab;
        }
    }

    public void Create(string prefabID, string instanceID, LocalizedString content, Action button1, Action button2)
    {
        if (_activePopups.ContainsKey(instanceID))
        {
            Destroy(instanceID);
        }

        if (canvas == null)
        {
            CanvasCreator canvasCreator = new CanvasCreator();
            canvas = canvasCreator.Create(false);
        }

        GameObject popupGO = GameObject.Instantiate(_popupsPrefab[prefabID], canvas.transform);
        Popup popup = popupGO.GetComponent<Popup>();
        popup.Setup(instanceID , content, button1, button2);
        _activePopups.Add(instanceID, popupGO);
    }

    public void Create(string prefabID, string instanceID, LocalizedString content) => Create(prefabID, instanceID, content, null, null);

    public void Show(string instanceID)
    {
        if (_activePopups.ContainsKey(instanceID))
        {
            _activePopups[instanceID].SetActive(true);
        }
        else
        {
            Debug.LogError($"[PopupService] Popup with instanceID {instanceID} not found.");
        }
    }

    public void Hide(string instanceID)
    {
        if (_activePopups.ContainsKey(instanceID))
        {
            _activePopups[instanceID].SetActive(false);
        }
        else
        {
            Debug.LogError($"[PopupService] Popup with instanceID {instanceID} not found.");
        }
    }

    public void Destroy(string instanceID)
    {
        if (_activePopups.ContainsKey(instanceID))
        {
            GameObject popupGO = _activePopups[instanceID];
            GameObject.Destroy(popupGO);
            _activePopups.Remove(instanceID);
        }
        else
        {
            Debug.LogError($"[PopupService] Popup with instanceID {instanceID} not found.");
        }
    }
}
