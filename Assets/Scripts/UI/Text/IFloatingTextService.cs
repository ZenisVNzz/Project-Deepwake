using UnityEngine;

public interface IFloatingTextService : IUIService
{
    void Create(string prefabID, string instanceID, string content, Vector3 position);
}
