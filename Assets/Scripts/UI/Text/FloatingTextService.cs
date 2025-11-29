using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class FloatingTextService : IFloatingTextService
{
    private Dictionary<string, GameObject> _uiPrefab = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _activeUI = new Dictionary<string, GameObject>();
    private GameObject canvas;

    public FloatingTextService()
    {
        FloatingTextContainer ui = ResourceManager.Instance.GetAsset<FloatingTextContainer>("FloatingTextContainer");
        foreach (var entry in ui.Text)
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

        if (uiGO.GetComponent<FloatingText>())
        {
            uiGO.GetComponent<FloatingText>().SetText(content);
        }

        _activeUI.Add(instanceID, uiGO);
    }

    public void Create(string prefabID, string instanceID, LocalizedString content, Vector3 position, bool Moving)
    {
        if (canvas == null)
        {
            CanvasCreator canvasCreator = new CanvasCreator();
            canvas = canvasCreator.Create(true);
        }

        GameObject uiGO = GameObject.Instantiate(_uiPrefab[prefabID], canvas.transform);
        uiGO.transform.position = position;

        if (uiGO.GetComponent<FloatingText>())
        {
            uiGO.GetComponent<FloatingText>().SetText(content.GetLocalizedString());
        }

        if (Moving)
        {
            uiGO.AddComponent<ObjectMove>();
        }

        _activeUI.Add(instanceID, uiGO);
    }

    public void Create(string prefabID, string instanceID, LocalizedString content, Vector3 position) => Create(prefabID, instanceID, content.GetLocalizedString(), position);
    public void Create(string prefabID, string instanceID, LocalizedString content) => Create(prefabID, instanceID, content.GetLocalizedString(), Vector3.zero);

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

    public void Destroy(string instanceID, float time)
    {
        if (_activeUI.ContainsKey(instanceID))
        {
            GameObject uiGO = _activeUI[instanceID];
            GameObject.Destroy(uiGO, time);
            _activeUI.Remove(instanceID);
        }
        else
        {
            Debug.LogError($"[SingleUIService] UI with instanceID {instanceID} not found.");
        }
    }
}
