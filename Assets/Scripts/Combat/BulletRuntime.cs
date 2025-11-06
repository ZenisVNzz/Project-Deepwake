using UnityEngine;

public class BulletRuntime : MonoBehaviour
{
    protected void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
