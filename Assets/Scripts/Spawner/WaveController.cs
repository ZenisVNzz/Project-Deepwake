using System.Collections;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private EnemySpawnTable enemySpawnTable;
    [SerializeField] private Transform ship;
    private EnemySpawner enemySpawner;

    private int currentWave = 0;
    public int CurrentWave => currentWave;

    private void Awake()
    {
        enemySpawner = new EnemySpawner(enemySpawnTable, ship);
    }

    private IEnumerator WaitForAllEnemyAreDead()
    {
        yield return new WaitForSeconds(7f);

        while (!enemySpawner.AreAllEnemiesDead())
        {
            yield return new WaitForSeconds(1f);
        }

        GameController.Instance.gameStateMachine.ChangeState<WaveResultStage>();
    }

    public void IncreaseDifficulty()
    {
        enemySpawner.IncreaseDifficulty();
    }

    public void SpawnNextWave()
    {
        currentWave++;
        Debug.Log($"Wave {currentWave} incoming");
        StartCoroutine(enemySpawner.SpawnEnemy());
        StartCoroutine(WaitForAllEnemyAreDead());
    }
}
