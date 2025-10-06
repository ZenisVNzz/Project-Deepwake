using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class SingleUIService : ISingleUIService
{
    private Dictionary<string, GameObject> _uiPrefab = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _activeUI = new Dictionary<string, GameObject>();
    private GameObject canvas;

    public SingleUIService()
    {
        SingleUIContainer ui = ResourceManager.Instance.GetAsset<SingleUIContainer>("SingleUIContainer");
        foreach (var entry in ui.UI)
        {
            _uiPrefab[entry.ID] = entry.Prefab;
        }
    }

    public void Create(string prefabID, string instanceID, string content, Vector3 position)
    {
        if (canvas == null)
        {
            CanvasCreator canvasCreator = new CanvasCreator();
            canvas = canvasCreator.Create(true);
        }

        GameObject uiGO = GameObject.Instantiate(_uiPrefab[prefabID], canvas.transform);
        uiGO.transform.position = position;

        if (uiGO.GetComponent<FloatingDamage>())
        {
            uiGO.GetComponent<FloatingDamage>().SetDamage(content);
        }
            _activeUI.Add(instanceID, uiGO);
    }

    public void Create(string prefabID, string instanceID, LocalizedString content) => Create(prefabID, instanceID, content.GetLocalizedString(), new Vector3(0, 0));

    public void Show(string instanceID)
    {
        if (_activeUI.ContainsKey(instanceID))
        {
            _activeUI[instanceID].SetActive(true);
        }
        else
        {
            Debug.LogError($"[SingleUIService] UI with instanceID {instanceID} not found.");
        }
    }

    public void Hide(string instanceID)
    {
        if (_activeUI.ContainsKey(instanceID))
        {
            _activeUI[instanceID].SetActive(false);
        }
        else
        {
            Debug.LogError($"[SingleUIService] UI with instanceID {instanceID} not found.");
        }
    }

    public void Destroy(string instanceID)
    {
        if (_activeUI.ContainsKey(instanceID))
        {
            GameObject uiGO = _activeUI[instanceID];
            GameObject.Destroy(uiGO);
            _activeUI.Remove(instanceID);
        }
        else
        {
            Debug.LogError($"[SingleUIService] UI with instanceID {instanceID} not found.");
        }
    }
}
