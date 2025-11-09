using Mirror;
using System.Collections;
using UnityEngine;

public class BulletRuntime : NetworkBehaviour
{
    [SerializeField] private float delay = 5f;

    protected void OnBecameInvisible()
    {
        if (!NetworkServer.active) return;
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        NetworkServer.Destroy(this.gameObject);
    }
}
