using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBossDataBase", menuName = "Data/EnemyBossDataBase")]
public class EnemyBossDataBase : ScriptableObject
{
    public List<Boss> bosses = new List<Boss>();
}

[Serializable]
public class Boss
{
    public string id;
    public GameObject prefab;
}
