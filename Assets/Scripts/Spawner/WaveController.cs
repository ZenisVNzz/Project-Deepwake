using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private EnemySpawnTable enemySpawnTable;
    [SerializeField] private Transform ship;
    private EnemySpawner enemySpawner;
    private float delayBetweenWaves = 5f;

    private bool waitingNextWave = false;
    private int currentWave = 0;

    private void Awake()
    {
        enemySpawner = new EnemySpawner(enemySpawnTable, ship, currentWave);
        SpawnNextWave();
    }

    private void Update()
    {
        if (!waitingNextWave && enemySpawner.AreAllEnemiesDead())
        {
            waitingNextWave = true;
            Invoke(nameof(SpawnNextWave), delayBetweenWaves);
        }
    }

    private void SpawnNextWave()
    {
        currentWave++;
        Debug.Log($"Wave {currentWave} incoming");
        StartCoroutine(enemySpawner.SpawnEnemy());
        waitingNextWave = false;
    }
}
