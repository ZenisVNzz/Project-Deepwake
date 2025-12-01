using Mirror;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public static BossSpawner Instance { get; private set; }
    private EnemyBossDataBase bossDataBase;

    public void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        bossDataBase = ResourceManager.Instance.GetAsset<EnemyBossDataBase>("EnemyBossDataBase");
    }

    public GameObject SpawnBoss(string bossID, Vector3 position)
    {
        GameObject bossPrefab = bossDataBase.bosses.Find(b => b.id == bossID)?.prefab;
        if (bossPrefab != null)
        {
            GameObject bossInstance = Instantiate(bossPrefab, position, Quaternion.identity);
            NetworkServer.Spawn(bossInstance);
            EnemyBoss bossComponent = bossInstance.GetComponent<EnemyBoss>();
            bossComponent.OnSpawn();

            return bossInstance;
        }
        else
        {
            Debug.LogError($"Boss with ID {bossID} not found in the database.");
            return null;
        }
    }
}
