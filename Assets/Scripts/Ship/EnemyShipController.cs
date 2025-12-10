using Mirror;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShipController : MonoBehaviour, IAttackable
{
    public static EnemyShipController Instance { get; private set; }
    [SerializeField] private Transform follower;
    public event Action onDeath;

    public List<EnemyCannonController> cannons = new();
    public List<Transform> RepairPot = new();

    public BossStatusUI BossStatusUI;

    public GameObject cannonObject;
    public List<Transform> cannonSpawnPos = new();

    public float MaxHP = 3000f;
    public float HP = 3000f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (Transform pos in cannonSpawnPos)
        {
            GameObject cannonGO = Instantiate(cannonObject, pos.position, pos.rotation, pos);
            EnemyCannonController cannonController = cannonGO.GetComponent<EnemyCannonController>();
            NetworkServer.Spawn(cannonGO);
            cannons.Add(cannonController);
        }
    }

    private void Start()
    {
        HP = MaxHP;
        BossStatusUI.SetData("GHOST PIRATE SHIP", MaxHP);
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

    public void TakeDamage(float dmg, Vector3 knockback, ICharacterRuntime attacker)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            HP = 0;
            onDeath?.Invoke();
        }

        BossStatusUI.UpdateHealth(HP);
    }

    public void TakeDamage(float dmg, Vector3 knockback) => TakeDamage(dmg, knockback, null);

    public void Repair(float amount)
    {
        HP += amount;
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }

        BossStatusUI.UpdateHealth(HP);
    }
}
