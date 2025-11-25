using Mirror;
using System.Collections.Generic;
using UnityEngine;
using static EnemyFlyingMovement;

public class GhostPirateMovement : MonoBehaviour, IAIMove
{
    public List<EnemyCannonController> Cannon;
    private EnemyCannonController currentCannon = null;
    private IState enemyState;

    public bool isControllingCannon = false;
    public bool haveReachedTarget = false;
    public bool CanMove = true;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyState = GetComponent<EnemyController>().enemyState;
    }

    [Server]
    public void Move(float moveSpeed)
    {
        if (!CanMove)
        {
            rb.linearVelocity = Vector2.zero;
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
                return;
            }
            else
            {
                Vector2 dir = (currentCannon.transform.position - this.transform.position).normalized;
                rb.linearVelocity = dir * moveSpeed;
                isControllingCannon = false;
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

    private void GetRandomCracked()
    {

    }

    public Vector2 GetDir()
    {
        Vector2 vel = rb.linearVelocity;
        return vel.normalized;
    }

    public bool HaveReachedTarget() => haveReachedTarget;
}
