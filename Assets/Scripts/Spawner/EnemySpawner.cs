using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner
{
    private EnemySpawnTable enemySpawnTable;
    private Transform ship;
    private int difficultyMultiplier;
    private EnemyLevelModifier levelModifier;

    private float minSpawnDistance = 11f;
    private float maxSpawnDistance = 13f;

    private List<GameObject> activeEnemies = new List<GameObject>();

    public EnemySpawner(EnemySpawnTable enemySpawnTable, Transform ship, int difficultyMultiplier)
    {
        this.enemySpawnTable = enemySpawnTable;
        this.ship = ship;
        this.difficultyMultiplier = difficultyMultiplier;
        levelModifier = new EnemyLevelModifier();
    }

    public IEnumerator SpawnEnemy()
    {
        int count = Random.Range(enemySpawnTable.minSpawn, enemySpawnTable.maxSpawn + 1);

        for (int i = 0; i < count; i++)
        {
            var enemyData = levelModifier.ModifyStats(enemySpawnTable.GetRandomEnemy(), difficultyMultiplier);
            var spawnPos = GetRandomSpawnPos();
            var enemy = GameObject.Instantiate(enemyData.prefab, spawnPos, Quaternion.identity);
            enemy.transform.SetParent(ship, worldPositionStays: true);
            var CharInstaller = enemy.GetComponent<CharacterInstaller>();
            CharInstaller.SetData(enemyData);
            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRandomSpawnPos()
    {
        float angle = Random.Range(0f, 360f);

        float min2 = minSpawnDistance * minSpawnDistance;
        float max2 = maxSpawnDistance * maxSpawnDistance;
        float distance = Mathf.Sqrt(Random.Range(min2, max2));

        float rad = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(rad) * distance, Mathf.Sin(rad) * distance);
        return (Vector2)ship.position + offset;
    }

    public bool AreAllEnemiesDead() => activeEnemies.TrueForAll(e => e == null);
}
