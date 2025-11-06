using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    public VFXHandler Handler { get; private set; }

    private Dictionary<string, VFXPool> vfxPools = new();

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
        var allVFX = Resources.LoadAll<VFXData>("VFXData");
        foreach (var data in allVFX)
        {
            if (data.prefab != null && !vfxPools.ContainsKey(data.vfxID))
            {
                var pool = new VFXPool(data.prefab, data.poolSize, transform);
                vfxPools.Add(data.vfxID, pool);
            }
        }
        Debug.Log($"[VFXManager] Initialized {vfxPools.Count} VFX pools.");
    }

    public BaseVFX GetFromPool(string id)
    {
        if (vfxPools.TryGetValue(id, out var pool))
            return pool.Get();
        return null;
    }

    public void ReturnToPool(BaseVFX vfx)
    {
        foreach (var kvp in vfxPools)
        {
            if (kvp.Value != null && vfx.name.Contains(kvp.Key))
            {
                kvp.Value.Return(vfx);
                return;
            }
        }
    }
}