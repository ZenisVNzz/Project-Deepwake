using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestPrefabList", menuName = "Data/ChestPrefabList")]
public class ChestPrefabList : ScriptableObject
{
    public List<ChestData> chestPrefabs;
}

[Serializable]
public class ChestData
{
    public ChestTier tier;
    public GameObject prefab;
}
