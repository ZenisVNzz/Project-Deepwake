using UnityEngine;

[CreateAssetMenu(menuName = "Data/NodeType")]
public class NodeType : ScriptableObject
{
    public NodeTypes NodeTypes;
    public Sprite image;
    public int rate;
}

public enum NodeTypes
{
    Sea,
    Shop,
    Monster,
    Treasure,
    Boss
}
