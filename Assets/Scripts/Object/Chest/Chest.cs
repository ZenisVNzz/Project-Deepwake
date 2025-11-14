using DG.Tweening;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chest : NetworkBehaviour
{
    [SyncVar] public int parentIndex;

    [SerializeField] private Transform prefabParent;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float lootSpawnDelay = 0.7f;

    [Header("Loot Settings")]
    [SerializeField] private GameObject defaultPickupPrefab;
    [Min(0)] [SerializeField] private int minDrops = 1;  
    [Min(0)] [SerializeField] private int maxDrops = 3; 

    [SerializeField] private Transform[] dropPoints;
    [SerializeField] private float spawnRadius = 0.5f;

    [Header("Pickup Delay")]
    [SerializeField] private float pickupDelay = 0.35f;

    [Header("Loot Table (rate-based 0..1)")]
    [SerializeField] private List<LootDefinition> lootTable = new List<LootDefinition>();

    private Interactable _interactable;
    [SyncVar] private bool _opened;
    private bool _lootSpawned;


    public override void OnStartClient()
    {
        base.OnStartClient();

        Transform chestParent = GameObject.Find("ChestSpawnPoints").transform;

        List<Transform> shipSpawnPoints = new();
        for (int i = 0; i < chestParent.childCount; i++)
        {
            shipSpawnPoints.Add(chestParent.GetChild(i));
        }

        gameObject.transform.SetParent(shipSpawnPoints[parentIndex], false);
    }

    private void Awake()
    {
        if (prefabParent == null)
            prefabParent = this.transform;

        _interactable = GetComponentInChildren<Interactable>();
        if (animator == null) animator = GetComponent<Animator>();

        if (_interactable != null)
            _interactable.Register(OnInteractWithPlayer);
    }

    [Server]
    private void OnInteractWithPlayer(NetworkConnectionToClient player)
    {
        if (_opened) return;
        _opened = true;

        if (animator != null)
            animator.Play("Chest_Open");

        if (lootSpawnDelay > 0f)
        {
            Invoke(nameof(SpawnLootOnce), lootSpawnDelay);
            Invoke(nameof(DestroySelf), lootSpawnDelay + 1f);
        }
        else
        {
            var clips = animator != null ? animator.GetCurrentAnimatorClipInfo(0) : null;
            float delay = (clips != null && clips.Length > 0 && clips[0].clip != null) ? clips[0].clip.length : 0.7f;
            Invoke(nameof(SpawnLootOnce), delay);
            Invoke(nameof(DestroySelf), delay + 1f);
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

        if (lootTable == null || lootTable.Count == 0)
        {
            Debug.LogWarning("[Chest] Loot table is empty.");
            return;
        }

        Vector3 GetCenter() => GetDropPoint();

        LootSpawner.SpawnByRate(
            table: lootTable,
            parent: prefabParent != null ? prefabParent.transform.parent : transform,
            getCenter: GetCenter,
            radius: spawnRadius,
            pickupDelay: pickupDelay,
            launchFrom: transform.position,     
            defaultPickupPrefab: defaultPickupPrefab,
            minDrops: minDrops,
            maxDrops: maxDrops);
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

    [Server]
    private void DestroySelf()
    {
        RpcFadeAndDestroy();
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcFadeAndDestroy()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            sr.DOFade(0f, 3f).OnComplete(() => Destroy(gameObject)); 
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