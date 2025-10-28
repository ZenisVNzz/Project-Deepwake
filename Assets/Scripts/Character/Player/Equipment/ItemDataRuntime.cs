using UnityEngine;
using System.Threading;

public class ItemDataRuntime : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    private int _claimed = 0;

    private Collider2D _trigger;
    private SpriteRenderer _visual;

    private void Awake()
    {
        _trigger = GetComponent<Collider2D>();
        _visual = GetComponent<SpriteRenderer>();

        if (_trigger == null)
            Debug.LogWarning("[ItemDataRuntime] Missing Collider2D on item.");
        else
            _trigger.isTrigger = true;
    }

    public void SetData(ItemData data)
    {
        _itemData = data;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<IPlayerRuntime>();
        if (player == null) return;

        if (Interlocked.CompareExchange(ref _claimed, 1, 0) != 0)
            return;

        if (_trigger) _trigger.enabled = false;
        if (_visual) _visual.enabled = false;

        bool added = false;
        try
        {
            added = (player.PlayerInventory != null) && player.PlayerInventory.AddItem(_itemData);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[ItemDataRuntime] AddItem exception: {ex}");
        }

        if (added)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(ReleaseAfterFail());
        }
    }

    private System.Collections.IEnumerator ReleaseAfterFail()
    {
        yield return new WaitForFixedUpdate();
        if (_trigger) _trigger.enabled = true;
        if (_visual) _visual.enabled = true;
        Interlocked.Exchange(ref _claimed, 0);
    }
}
