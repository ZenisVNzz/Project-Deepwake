using UnityEngine;

public interface IUIService
{
    void Create(string prefabID, string instanceID, string content);
    void Show(string id);
    void Hide(string id);
    void Destroy(string id);
}
