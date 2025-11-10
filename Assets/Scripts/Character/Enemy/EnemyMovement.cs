using Mirror.BouncyCastle.Math.Field;
using Pathfinding;
using PlayFab.MultiplayerModels;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IAIMove
{
    private Transform target;
    private float nextWaypointDistance = 0.3f;
    private float stopDistance = 0.9f;
    private float chaseDistance = 15f;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;

    private bool haveReachedTarget = false;

    private void Awake()
    {
        this.seeker = GetComponent<Seeker>();
        this.rb = GetComponent<Rigidbody2D>();
       
        target = GameObject.FindGameObjectWithTag("Player").transform;
        InitPath();
    }

    private void InitPath()
    {
        UpdatePath();
        StartCoroutine(UpdatePathCour());
    }

    private IEnumerator UpdatePathCour()
    {
        while (true)
        {
            UpdatePath();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }    

    public void Move(float moveSpeed)
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) return;

        float distanceToPlayer = Vector2.Distance(rb.position, target.position);
        if (distanceToPlayer > chaseDistance)
        {
            return;
        }

        if (distanceToPlayer <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            haveReachedTarget = true;
            return;
        }
        else
        {
            haveReachedTarget = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            currentWaypoint++;
    }

    public void Move(Vector2 input, float moveSpeed, bool isMoveOnSlope) => Move(moveSpeed);

    public Vector2 GetDir()
    {
        if (path == null || path.vectorPath == null || path.vectorPath.Count < 2)
            return Vector2.zero;

        Vector2 currentPos = rb.position;
        Vector2 targetPos = path.vectorPath[currentWaypoint];

        Vector2 dir = (targetPos - currentPos).normalized;
        return dir;
    }

    public bool HaveReachedTarget () => haveReachedTarget;
}
