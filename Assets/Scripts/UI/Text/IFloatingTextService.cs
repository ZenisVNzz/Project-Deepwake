using UnityEngine;
using UnityEngine.Localization;

public interface IFloatingTextService : IUIService
{
    void Create(string prefabID, string instanceID, string content, Vector3 position);
    void Create(string prefabID, string instanceID, LocalizedString content, Vector3 position);
    void Create(string prefabID, string instanceID, LocalizedString content, Vector3 position, bool Moving);
}
