using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : MonoBehaviour
{
    private readonly List<BaseVFX> active = new List<BaseVFX>();

    public void Track(BaseVFX vfx, float duration)
    {
        if (vfx == null) return;
        active.Add(vfx);
        StartCoroutine(DeactivateAfter(vfx, duration));
    }

    private IEnumerator DeactivateAfter(BaseVFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (vfx != null)
        {
            vfx.Stop();
            VFXManager.Instance.ReturnToPool(vfx);
        }
        active.Remove(vfx);
    }
}