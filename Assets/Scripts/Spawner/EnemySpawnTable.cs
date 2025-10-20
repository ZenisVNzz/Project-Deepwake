using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnTable", menuName = "Data/EnemySpawnTable")]
public class EnemySpawnTable : ScriptableObject
{
    public List<CharacterData> characterData;
    public int minSpawn;
    public int maxSpawn;

    public CharacterData GetRandomEnemy()
    {
        int enemyCount = characterData.Count;
        int index = Random.Range(0, enemyCount);
        return characterData[index];
    }
}
