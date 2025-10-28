using System.Threading;
using UnityEngine;

public class ItemDataRuntime : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    private float _pickupEnableTime;

    private int _claimed = 0;

    public void SetData(ItemData data)
    {
        _itemData = data;
    }

    public void SetPickupDelay(float seconds)
    {
        _pickupEnableTime = Time.time + Mathf.Max(0f, seconds);
    }

    private bool CanPickupNow()
    {
        return Time.time >= _pickupEnableTime;
    }

    private void TryPickup(IPlayerRuntime player)
    {
        if (!CanPickupNow() || player == null) return;

        if (Interlocked.CompareExchange(ref _claimed, 1, 0) != 0)
            return;

        bool added = player.PlayerInventory != null && player.PlayerInventory.AddItem(_itemData);
        if (added)
        {
            Destroy(gameObject);
        }
        else
        {
            Interlocked.Exchange(ref _claimed, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<IPlayerRuntime>();
        if (player == null) return;

        TryPickup(player);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<IPlayerRuntime>();
        if (player == null) return;

        TryPickup(player);
    }
}
