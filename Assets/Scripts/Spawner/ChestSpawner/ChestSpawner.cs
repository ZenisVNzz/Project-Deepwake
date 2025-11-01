using System.Collections.Generic;
using UnityEngine;

public enum ChestTier
{
    Tier1,
    Tier2,
    Tier3,
    Tier4
}

public class ChestSpawner
{
    private List<Transform> shipSpawnPoints = new();
    private Dictionary<ChestTier, GameObject> chestPrefab = new();

    private float Tier1Chance = 0.5f;
    private float Tier2Chance = 0.3f;
    private float Tier3Chance = 0f;
    private float Tier4Chance = 0f;

    public ChestSpawner()
    {
        ChestPrefabList chestPrefabList = ResourceManager.Instance.GetAsset<ChestPrefabList>("ChestPrefabList");
        foreach (var chestData in chestPrefabList.chestPrefabs)
        {
            chestPrefab[chestData.tier] = chestData.prefab;
        }

        shipSpawnPoints.AddRange(GameObject.Find("ChestSpawnPoints").GetComponentsInChildren<Transform>());

        Tier1Chance = 0.5f;
        Tier2Chance = 0.3f;
        Tier3Chance = 0f;
        Tier4Chance = 0f;
    }

    public void IncreaseChestRate(float multipler)
    {
        if (Tier1Chance >= 0f)
        {
            Tier1Chance -= 0.02f * multipler;
        }
        if (Tier2Chance >= 0.1f)
        {
            Tier2Chance -= 0.01f * multipler;
        }
        if (Tier3Chance <= 0.5f)
        {
            Tier3Chance += 0.02f * multipler;
        }
        if (Tier4Chance <= 0.25f)
        {
            Tier4Chance += 0.01f * multipler;
        }
    }

    public void SpawnChestOnShip(int spawnCount = 1)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            ChestTier tier = GetRandomTier();
            GameObject chest = chestPrefab[tier];
            GameObject chestInstance = GameObject.Instantiate(chest, shipSpawnPoints[i]);
        }
    }

    private ChestTier GetRandomTier()
    {
        float index = Random.Range(0f, 1f);
        if (index <= Tier4Chance)
        {
            return ChestTier.Tier4;
        }
        else if (index <= Tier3Chance)
        {
            return ChestTier.Tier3;
        }
        else if (index <= Tier2Chance)
        {
            return ChestTier.Tier2;
        }
        else
        {
            return ChestTier.Tier1;
        }
    }
}
