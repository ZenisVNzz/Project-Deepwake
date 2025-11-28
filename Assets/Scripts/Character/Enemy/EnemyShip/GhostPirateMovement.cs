using Mirror;
using System.Collections.Generic;
using UnityEngine;
using static EnemyFlyingMovement;

public class GhostPirateMovement : MonoBehaviour, IAIMove
{
    public List<EnemyCannonController> Cannon;
    public List<Transform> RepairPot;
    public EnemyCannonController currentCannon = null;
    private IState enemyState;

    [Header("Movement Settings")]
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 20f;

    public bool isControllingCannon = false;
    public bool haveReachedTarget = false;
    public bool CanMove = true;

    private Rigidbody2D rb;

    public float repair = 80f;
    public bool active = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyState = GetComponent<EnemyController>().enemyState;
    }

    [Server]
    public void Move(float moveSpeed)
    {
        if (EnemyShipController.Instance.HP + 400 < EnemyShipController.Instance.MaxHP)
        {
            if (isControllingCannon)
            {
                isControllingCannon = false; 
                currentCannon.ReleaseCannon();
                currentCannon = null;
            }

            Transform repairTarget = GetRandomRepairTarget();
            float distanceToRepair = Vector2.Distance(repairTarget.position, this.transform.position);

            if (distanceToRepair < 0.7f)
            {
                EnemyShipController.Instance.Repair(repair * Time.fixedDeltaTime);
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                Vector2 dir = (repairTarget.position - this.transform.position).normalized;
                rb.linearVelocity = dir * moveSpeed;
            }
            return;
        }

        if (!CanMove || !active)
        {
            return;
        }

        if (currentCannon == null)
        {
            currentCannon = GetRandomCannon();
        }
        else
        {
            float distance = Vector2.Distance(currentCannon.transform.position, this.transform.position);
            if (distance < 0.65f && !isControllingCannon)
            {
                currentCannon.UseCannon(this.GetComponent<NetworkIdentity>());
                isControllingCannon = true;
                rb.linearVelocity = Vector2.zero;
                return;
            }
            else
            {
                Vector2 dir = (currentCannon.transform.position - this.transform.position).normalized;
                rb.linearVelocity = dir * moveSpeed;
            }
        }
    }

    public void CmdMove(Vector2 input, float moveSpeed, bool isMoveOnSlope) => Move(moveSpeed);

    private EnemyCannonController GetRandomCannon()
    {
        if (Cannon.Count == 0) return null;
        int randIndex = Random.Range(0, Cannon.Count);
        if (Cannon[randIndex].CurEnemy != null)
        {
            return GetRandomCannon();
        }
        return Cannon[randIndex];
    }

    private Transform GetRandomRepairTarget()
    {
        if (RepairPot.Count == 0) return null;
        int randIndex = Random.Range(0, RepairPot.Count);
        return RepairPot[randIndex];
    }

    public Vector2 GetDir()
    {
        Vector2 vel = rb.linearVelocity;
        return vel.normalized;
    }

    public bool HaveReachedTarget() => haveReachedTarget;
}
