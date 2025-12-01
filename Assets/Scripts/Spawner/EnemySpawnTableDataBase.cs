using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnTableDataBase", menuName = "Data/Database/EnemySpawnTableDataBase")]
public class EnemySpawnTableDataBase : ScriptableObject
{
    public List<EnemySpawnTable> enemySpawnTables = new List<EnemySpawnTable>();
}
