using UnityEngine;

public class EnemyShipController : MonoBehaviour
{
    public static EnemyShipController Instance { get; private set; }
    [SerializeField] private Transform follower;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

    public void SetChild(Transform child, bool worldPositionStay, bool ResetPostion)
    {
        if (follower == null) return;
        Vector3 originalScale = child.localScale;
        child.SetParent(follower, worldPositionStay);


        if (ResetPostion)
        {
            child.localPosition = Vector3.zero;
        }

        child.localScale = originalScale;
    }
}
