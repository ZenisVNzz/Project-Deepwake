using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapNodeData
{
    public Vector2Int gridPos;
    public NodeType nodeType;

    public void OnSelect()
    {
        Debug.Log($"Selected node {nodeType.NodeTypes}");
    }
}
