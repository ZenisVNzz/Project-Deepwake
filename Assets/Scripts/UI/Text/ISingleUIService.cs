using UnityEngine;

public interface ISingleUIService : IUIService
{
    void Create(string prefabID, string instanceID, string content, Vector3 position);
}
