using UnityEngine;

public static class VFXSpawner
{
    public static BaseVFX Spawn(string id, Vector3 position)
    {
        var listSO = ResourceManager.LoadResource<VFXDataListSO>("VFX/VFXDataList");
        if (listSO == null)
        {
            Debug.LogWarning("[VFXSpawner] VFXDataList not found.");
            return null;
        }

        var data = listSO.vfxList.Find(v => v != null && v.vfxID == id);
        if (data == null)
        {
            Debug.LogWarning($"[VFXSpawner] VFXData with id '{id}' not found.");
            return null;
        }

        var vfx = VFXManager.Instance.GetFromPool(id);
        if (vfx == null)
        {
            Debug.LogWarning($"[VFXSpawner] Could not get vfx from pool for id {id}");
            return null;
        }

        vfx.Initialize(data);
        vfx.Play(position);

        if (data.sfx != null)
            AudioManager.PlayOneShot(data.sfx, position);

        VFXManager.Instance.Handler.Track(vfx, data.lifeTime);
        return vfx;
    }
}
