using System.Collections.Generic;
using UnityEngine;

public class VFXPool
{
    private readonly Queue<BaseVFX> pool = new();
    private readonly BaseVFX prefab;
    private readonly Transform parent;

    public VFXPool(BaseVFX prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < initialSize; i++)
        {
            var obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public BaseVFX Get()
    {
        var obj = pool.Count > 0 ? pool.Dequeue() : Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(true);
        obj.OnSpawned();
        return obj;
    }

    public void Return(BaseVFX obj)
    {
        obj.OnDespawned();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}