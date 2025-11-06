using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : MonoBehaviour
{
    private readonly List<BaseVFX> activeVFX = new();

    public void Track(BaseVFX vfx, float duration)
    {
        activeVFX.Add(vfx);
        StartCoroutine(ReturnAfterDelay(vfx, duration));
    }

    private IEnumerator ReturnAfterDelay(BaseVFX vfx, float delay)
    {
        yield return new WaitForSeconds(delay);
        vfx.Stop();
        activeVFX.Remove(vfx);
        VFXManager.Instance.ReturnToPool(vfx);
    }
}