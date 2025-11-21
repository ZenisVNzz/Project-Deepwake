using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public static EnvironmentSpawner Instance { get; private set; }

    [SerializeField] private Transform ship;
    private Dictionary<string, GameObject> Prefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        EnvironmentList environmentList = ResourceManager.Instance.GetAsset<EnvironmentList>("EnvironmentList");
        foreach (GameObject prefab in environmentList.Prefabs)
        {
            Prefabs[prefab.name] = prefab;
        }
    }

    public void Spawn(string id, Vector3 offset, bool stopShip)
    {
        Vector3 position = ship.position + offset;
        GameObject enviromentGO = Instantiate(Prefabs[id], position, Quaternion.identity);
        NetworkServer.Spawn(enviromentGO);

        if (stopShip)
        {
            ShipController shipController = ShipController.Instance;
            shipController.MoveToX(position.x + 1.5f, false);
        }
    }
}
