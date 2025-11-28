using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    private Dictionary<string, VFXPool> vfxPools = new Dictionary<string, VFXPool>();
    public VFXHandler Handler { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Handler = gameObject.AddComponent<VFXHandler>();

        LoadPools();
    }

    private void LoadPools()
    {
        // path: Assets/VFX/VFXDataList.asset
        var listSO = ResourceManager.LoadResource<VFXDataListSO>("VFX/VFXDataList");
        if (listSO == null)
        {
            Debug.LogWarning("[VFXManager] VFXDataList not found at Resources/VFX/VFXDataList");
            return;
        }

        var allVFX = listSO.vfxList;
        int added = 0;
        foreach (var data in allVFX)
        {
            if (data == null) continue;
            if (data.prefab == null)
            {
                Debug.LogWarning($"[VFXManager] VFXData {data.vfxID} has no prefab assigned.");
                continue;
            }
            if (!vfxPools.ContainsKey(data.vfxID))
            {
                var pool = new VFXPool(data.prefab, Mathf.Max(1, data.poolSize), transform);
                vfxPools.Add(data.vfxID, pool);
                added++;
            }
        }
        Debug.Log($"[VFXManager] Initialized {added} VFX pools.");
    }

    public BaseVFX GetFromPool(string id)
    {
        if (vfxPools.TryGetValue(id, out var pool))
            return pool.Get();
        Debug.LogWarning($"[VFXManager] No pool for id: {id}");
        return null;
    }

    public void ReturnToPool(BaseVFX vfx)
    {
        if (vfx == null) return;

        foreach (var kv in vfxPools)
        {
            var prefabName = kv.Value.GetType();
        }

        foreach (var kv in vfxPools)
        {
            var poolPrefabName = kv.Key;
            if (vfx.gameObject.name.Contains(poolPrefabName))
            {
                kv.Value.Return(vfx);
                return;
            }
        }

        vfx.gameObject.SetActive(false);
    }
}