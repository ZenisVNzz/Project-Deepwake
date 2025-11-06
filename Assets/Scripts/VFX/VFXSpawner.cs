using UnityEngine;
using System.Collections.Generic;

public static class VFXSpawner
{
    public static BaseVFX Spawn(string id, Vector3 position)
    {

        var vfxListSO = ResourceManager.Instance.GetAsset<VFXDataListSO>("VFXDataList");
        if (vfxListSO == null)
        {
            Debug.LogWarning("[VFXSpawner] VFXDataList not loaded.");
            return null;
        }

        var vfxList = vfxListSO.vfxList;
        var data = vfxList.Find(v => v.vfxID == id);
        if (data == null)
        {
            Debug.LogWarning($"[VFXSpawner] VFXData with id '{id}' not found in VFXDataList.");
            return null;
        }

        var vfx = VFXManager.Instance.GetFromPool(id);
        if (vfx == null) return null;

        vfx.Initialize(data);
        vfx.Play(position);

        AudioManager.Instance.PlaySound(data.sfx, position);
        VFXManager.Instance.Handler.Track(vfx, data.lifeTime);

        return vfx;
    }
}