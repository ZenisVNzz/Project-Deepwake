using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner
{
    private EnemySpawnTableDataBase enemySpawnTable;
    private Transform ship;
    private int difficultyMultiplier;
    private EnemyLevelModifier levelModifier;

    private float minSpawnDistance = 11f;
    private float maxSpawnDistance = 13f;

    private List<GameObject> activeEnemies = new List<GameObject>();

    public EnemySpawner(Transform ship, int difficultyMultiplier = 1)
    {
        this.ship = ship;
        this.enemySpawnTable = ResourceManager.Instance.GetAsset<EnemySpawnTableDataBase>("EnemySpawnTableDataBase");
        this.difficultyMultiplier = difficultyMultiplier;
        levelModifier = new EnemyLevelModifier();
    }

    public void IncreaseDifficulty()
    {
        difficultyMultiplier++;
    }

    public IEnumerator SpawnEnemy()
    {
        if (!NetworkServer.active) yield break;

        activeEnemies.Clear();
        EnemySpawnTable enemySpawnTable = GetRandomSpawnTable();
        int count = Random.Range(enemySpawnTable.minSpawn, enemySpawnTable.maxSpawn + 1);
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < count; i++)
        {
            var enemyData = enemySpawnTable.GetRandomEnemy();
            var enemyDataRuntime = levelModifier.ModifyStats(enemyData, difficultyMultiplier);
            var spawnPos = GetRandomSpawnPos();
            var enemy = GameObject.Instantiate(enemyData.prefab, spawnPos, Quaternion.identity);   
            CharacterInstaller characterInstaller = enemy.GetComponent<CharacterInstaller>();
            characterInstaller.SetData(enemyDataRuntime);
            characterInstaller.InitCharacter(); 

            NetworkServer.Spawn(enemy);

            activeEnemies.Add(enemy);
            yield return new WaitForSeconds(2f);
        }
    }

    private EnemySpawnTable GetRandomSpawnTable()
    {
        return enemySpawnTable.enemySpawnTables[Random.Range(0, enemySpawnTable.enemySpawnTables.Count)];
    }

    public void ClearAllEnemies()
    {
        if (!NetworkServer.active) return;
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
            {
                NetworkServer.Destroy(enemy);
            }
        }
        activeEnemies.Clear();
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

    public bool AreAllEnemiesDead()
    {
        activeEnemies.RemoveAll(e => e == null);

        return activeEnemies.All(e =>
        {
            var controller = e.GetComponent<IEnemyController>();
            return controller == null || controller.IsDead;
        });
    }
}
