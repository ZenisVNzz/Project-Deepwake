using System.Collections.Generic;
using UnityEngine;

public class VFXPool
{
    private readonly GameObject prefab;
    private readonly Queue<BaseVFX> pool = new Queue<BaseVFX>();
    private readonly Transform parent;

    public VFXPool(GameObject prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            var obj = GameObject.Instantiate(prefab, parent);
            obj.SetActive(false);

            var vfx = obj.GetComponent<BaseVFX>();
            if (vfx == null)
            {
                Debug.LogError($"[VFXPool] Prefab {prefab.name} missing BaseVFX component.");
                GameObject.Destroy(obj);
                continue;
            }
            pool.Enqueue(vfx);
        }
    }

    public BaseVFX Get()
    {
        BaseVFX vfx;
        if (pool.Count > 0)
        {
            vfx = pool.Dequeue();
            if (vfx == null) return CreateNew();
        }
        else
        {
            vfx = CreateNew();
        }

        vfx.gameObject.SetActive(true);
        return vfx;
    }

    public void Return(BaseVFX vfx)
    {
        vfx.gameObject.SetActive(false);
        pool.Enqueue(vfx);
    }

    private BaseVFX CreateNew()
    {
        var obj = GameObject.Instantiate(prefab, parent);
        var vfx = obj.GetComponent<BaseVFX>();
        if (vfx == null)
            Debug.LogError($"[VFXPool] Created instance does not have BaseVFX: {prefab.name}");
        return vfx;
    }
}