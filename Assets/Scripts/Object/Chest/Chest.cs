using System;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Transform prefabParent;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float lootSpawnDelay = 0.7f;

    [Header("Loot Settings")]
    [SerializeField] private GameObject defaultPickupPrefab;
    [SerializeField] private int minDrops = 1;
    [SerializeField] private int maxDrops = 3;

    [SerializeField] private Transform[] dropPoints;
    [SerializeField] private float spawnRadius = 0.5f;

    [Tooltip("Impulse applied to spawned item if it has a Rigidbody2D.")]
    [SerializeField] private float tossForce = 2.5f;

    [Header("Pickup Delay")]
    [SerializeField] private float pickupDelay = 0.35f;

    [Header("Loot Table (weighted)")]
    [SerializeField] private List<LootEntry> lootTable = new List<LootEntry>();

    private Interactable _interactable;
    private bool _opened; 
    private bool _lootSpawned; 
    private float _totalWeight;

    [Serializable]
    public class LootEntry
    {
        public ItemData item;
        [Min(0f)] public float weight = 1f;
        public GameObject pickupPrefab;
        public int quantity = 1;
    }

    private void Awake()
    {
        if (prefabParent == null)
            prefabParent = this.transform;

        _interactable = GetComponentInChildren<Interactable>();
        if (animator == null) animator = GetComponent<Animator>();

        _interactable.Register(OnInteractWithPlayer);

        RecalculateWeights();
    }

    private void RecalculateWeights()
    {
        _totalWeight = 0f;
        for (int i = 0; i < lootTable.Count; i++)
        {
            _totalWeight += Mathf.Max(0f, lootTable[i].weight);
        }
    }

    private void OnInteractWithPlayer(GameObject player)
    {
        if (_opened) return;
        _opened = true;

        if (animator != null)
            animator.Play("Chest_Open");

        if (lootSpawnDelay > 0f)
        {
            Invoke(nameof(SpawnLootOnce), lootSpawnDelay);
        }
        else
        {
            var clips = animator != null ? animator.GetCurrentAnimatorClipInfo(0) : null;
            float delay = (clips != null && clips.Length > 0 && clips[0].clip != null) ? clips[0].clip.length : 0.7f;
            Invoke(nameof(SpawnLootOnce), delay);
        }
    }

    public void AnimationEvent_SpawnLoot()
    {
        SpawnLootOnce();
    }

    private void SpawnLootOnce()
    {
        if (!_opened || _lootSpawned) return;
        _lootSpawned = true;

        if (lootTable == null || lootTable.Count == 0 || _totalWeight <= 0f)
        {
            Debug.LogWarning("[Chest] Loot table is empty or has zero total weight.");
            return;
        }

        int count = Mathf.Clamp(UnityEngine.Random.Range(minDrops, maxDrops + 1), 0, 99);
        for (int i = 0; i < count; i++)
        {
            var entry = PickRandomEntry();
            if (entry == null || entry.item == null) continue;

            int qty = Mathf.Max(1, entry.quantity);
            for (int q = 0; q < qty; q++)
                SpawnSinglePickup(entry);
        }
    }

    private LootEntry PickRandomEntry()
    {
        if (_totalWeight <= 0f) return null;

        float r = UnityEngine.Random.Range(0f, _totalWeight);
        float cum = 0f;
        for (int i = 0; i < lootTable.Count; i++)
        {
            var w = Mathf.Max(0f, lootTable[i].weight);
            cum += w;
            if (r <= cum) return lootTable[i];
        }
        return lootTable[lootTable.Count - 1];
    }

    private void SpawnSinglePickup(LootEntry entry)
    {
        var prefab = entry.pickupPrefab != null ? entry.pickupPrefab : defaultPickupPrefab;
        if (prefab == null)
        {
            Debug.LogWarning("[Chest] No pickup prefab assigned for loot.");
            return;
        }

        Vector3 basePos = GetDropPoint();
        Vector2 offset = UnityEngine.Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = basePos + new Vector3(offset.x, offset.y, 0f);

        var go = Instantiate(prefab, spawnPos, Quaternion.identity, prefabParent);

        var dataRuntime = go.GetComponent<ItemDataRuntime>();
        if (dataRuntime != null)
        {
            dataRuntime.SetData(entry.item);
            dataRuntime.SetPickupDelay(pickupDelay);
        }
        else
        {
            Debug.LogWarning("[Chest] Spawned pickup has no ItemDataRuntime component.");
        }

        var toss = go.GetComponent<PickupToss>();
        if (toss == null) toss = go.AddComponent<PickupToss>();
        toss.Launch(transform.position);
    }

    private Vector3 GetDropPoint()
    {
        if (dropPoints != null && dropPoints.Length > 0)
        {
            var p = dropPoints[UnityEngine.Random.Range(0, dropPoints.Length)];
            if (p != null) return p.position;
        }
        return transform.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.85f, 0.2f, 0.4f);
        Vector3 center = transform.position;
        if (dropPoints != null)
        {
            foreach (var t in dropPoints)
            {
                if (t == null) continue;
                Gizmos.DrawWireSphere(t.position, spawnRadius);
            }
        }
        else
        {
            Gizmos.DrawWireSphere(center, spawnRadius);
        }
    }
#endif
}