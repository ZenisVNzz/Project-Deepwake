using Mirror;
using UnityEngine;

public class PlayerNet : NetworkBehaviour
{
    [SyncVar]
    public string playerName = "Zenis";

    [Server]
    public void ChangeGameMapRequest(NodeTypes MapType)
    {
        Debug.Log($"[Server] Changing game map to {MapType}");

        ScriptableObject mapNodeData = ScriptableObject.CreateInstance(nameof(NodeType));
        if (mapNodeData is NodeType nodeType)
        {
            nodeType.NodeTypes = MapType;
        }

        MapNodeData mapData = new MapNodeData
        {
            nodeType = mapNodeData as NodeType
        };

        RpcChangeGameMap(mapData);
    }

    [ClientRpc]
    private void RpcChangeGameMap(MapNodeData mapData)
    {
        mapData.OnSelect();
    }
}
