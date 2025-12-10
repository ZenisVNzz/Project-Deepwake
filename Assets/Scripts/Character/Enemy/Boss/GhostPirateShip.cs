using Mirror;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GhostPirateShip : EnemyBoss
{
    public Transform target;
    public float acceleration = 0.2f;
    public float maxSpeed = 2f;
    public float minSpeed = 0.5f;
    public float slowDownDistance = 0.8f;

    private float currentSpeed = 0f;
    float newSpeed;
    private ObjectMove mover;

    public GameObject enemyPrefab;
    public List<Transform> spawnPoints = new();

    public CinemachineCamera cameraObject;

    private void Awake()
    {
        mover = GetComponent<ObjectMove>();
        EnemyShipController.Instance.onDeath += OnDeath;
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + new Vector3(0f, 10f, 0f);
        Vector3 toTarget = targetPos - transform.position;
        float distance = toTarget.magnitude;    

        if (distance > slowDownDistance)
        {
            newSpeed = Mathf.MoveTowards(mover.Speed, maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            newSpeed = Mathf.MoveTowards(mover.Speed, minSpeed, acceleration * Time.deltaTime);
        }

        if (mover == null)
            mover = GetComponent<ObjectMove>();

        if (mover != null)
        {
            float dir = Mathf.Sign(targetPos.x - transform.position.x);
            mover.Speed = dir * Mathf.Abs(newSpeed);
        }
    }

    [Server]
    public override void OnSpawn()
    {
        base.OnSpawn();
        ObjectCleaner.Instance.ObjectToClean.Add(this.gameObject);
        target = ShipController.Instance != null ? ShipController.Instance.transform : target;

        mover = GetComponent<ObjectMove>();
        currentSpeed = 0f;

        StartCoroutine(spawnEnemy());

        Debug.Log("Ghost Pirate Ship has spawned!");
        Invoke(nameof(OnSpawnDone), 5f);
    }

    private IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(2f);

        foreach (Transform t in spawnPoints)
        {
            GameObject enemyPrefab = Instantiate(this.enemyPrefab, t.position, Quaternion.identity);
            NetworkServer.Spawn(enemyPrefab);
            yield return new WaitForSeconds(1f);
        }
    }

    [Server]
    public override void OnSpawnDone()
    {
        base.OnSpawnDone();
        cameraObject.Priority = -99;
        Debug.Log("Ghost Pirate Ship spawn process completed!");
    }

    [Server]
    public override void OnDeath()
    {
        base.OnDeath();
        if (mover != null) mover.Speed = 0f;
        GameController.Instance.NextLevel();
        Debug.Log("Ghost Pirate Ship has been defeated!");
    }
}
