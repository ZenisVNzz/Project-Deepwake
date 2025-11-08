using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class LootSpawner
{
    public static void SpawnByRate(
        IList<LootDefinition> table,
        Transform parent,
        Func<Vector3> getCenter,
        float radius,
        float pickupDelay,
        Vector3 launchFrom,
        GameObject defaultPickupPrefab,
        int minDrops = -1,
        int maxDrops = -1)
    {
        if (NetworkServer.active == false) return;

        if (table == null || table.Count == 0)
        {
            Debug.LogWarning("[LootSpawner] Table is empty.");
            return;
        }

        var rolled = new List<LootDefinition>();
        for (int i = 0; i < table.Count; i++)
        {
            var def = table[i];
            if (def == null || def.item == null) continue;

            float p = Mathf.Clamp01(def.rate);
            if (UnityEngine.Random.value <= p)
            {
                int qty = Mathf.Max(1, def.quantity);
                for (int q = 0; q < qty; q++)
                    rolled.Add(def);
            }
        }

        if (minDrops >= 0 || maxDrops >= 0)
        {
            int min = minDrops >= 0 ? minDrops : 0;
            int max = maxDrops >= 0 ? Mathf.Max(min, maxDrops) : int.MaxValue;

            if (rolled.Count > max)
                TrimListRandomInPlace(rolled, max);

            if (rolled.Count < min)
            {
                int need = min - rolled.Count;
                TopUpByRate(table, need, rolled);
            }
        }

        for (int i = 0; i < rolled.Count; i++)
        {
            var def = rolled[i];
            Vector3 center = getCenter != null ? getCenter() : Vector3.zero;
            Vector2 offset = UnityEngine.Random.insideUnitCircle * Mathf.Max(0f, radius);
            Vector3 spawnPos = center + new Vector3(offset.x, offset.y, 0f);

            SpawnPickup(
                prefab: def.pickupPrefab != null ? def.pickupPrefab : defaultPickupPrefab,
                item: def.item,
                spawnPos: spawnPos,
                parent: parent,
                pickupDelay: pickupDelay,
                launchFrom: launchFrom);
        }
    }

    public static GameObject SpawnPickup(
        GameObject prefab,
        ItemData item,
        Vector3 spawnPos,
        Transform parent,
        float pickupDelay,
        Vector3 launchFrom)
    {
        if (prefab == null)
        {
            Debug.LogWarning("[LootSpawner] No pickup prefab provided.");
            return null;
        }

        var go = UnityEngine.Object.Instantiate(prefab, spawnPos, Quaternion.identity, parent);

        var dataRuntime = go.GetComponent<ItemDataRuntime>();
        if (dataRuntime != null)
        {
            dataRuntime.SetData(item);
            dataRuntime.SetPickupDelay(pickupDelay);
            dataRuntime.SetParent(parent.GetComponent<NetworkIdentity>());
            NetworkServer.Spawn(go);
        }
        else
        {
            Debug.LogWarning("[LootSpawner] Spawned pickup has no ItemDataRuntime component.");
        }   

        var toss = go.GetComponent<PickupToss>();
        if (toss == null) toss = go.AddComponent<PickupToss>();
        toss.Launch(launchFrom);

        return go;
    }

    private static void TrimListRandomInPlace<T>(List<T> list, int keepCount)
    {
        while (list.Count > keepCount)
        {
            int idx = UnityEngine.Random.Range(0, list.Count);
            int last = list.Count - 1;
            list[idx] = list[last];
            list.RemoveAt(last);
        }
    }

    private static void TopUpByRate(IList<LootDefinition> table, int need, List<LootDefinition> output)
    {
        float total = 0f;
        for (int i = 0; i < table.Count; i++)
        {
            var d = table[i];
            if (d == null || d.item == null) continue;
            total += Mathf.Max(0f, d.rate);
        }
        if (total <= 0f) return;

        for (int n = 0; n < need; n++)
        {
            float r = UnityEngine.Random.Range(0f, total);
            float cum = 0f;
            LootDefinition picked = null;

            for (int i = 0; i < table.Count; i++)
            {
                var d = table[i];
                if (d == null || d.item == null) continue;

                float w = Mathf.Max(0f, d.rate);
                cum += w;
                if (r <= cum)
                {
                    picked = d;
                    break;
                }
            }
            if (picked == null) picked = table[table.Count - 1];
            output.Add(picked);
        }
    }
}