using Mirror.BouncyCastle.Math.Field;
using Pathfinding;
using PlayFab.MultiplayerModels;
using System.Collections;
using UnityEngine;

public class EnemyMovement : IMovable
{
    private Transform target;
    private float nextWaypointDistance = 0.3f;
    private float stopDistance = 0.8f;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D rb;
    private MonoBehaviour runner;

    public EnemyMovement(Seeker seeker, Rigidbody2D rb, MonoBehaviour runner)
    {
        this.seeker = seeker;
        this.rb = rb;
        this.runner = runner;
       
        target = GameObject.FindGameObjectWithTag("Player").transform;
        InitPath();
    }

    private void InitPath()
    {
        UpdatePath();
        runner.StartCoroutine(UpdatePathCour());
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

    public void Move()
    {
        if (path == null) return;
        if (currentWaypoint >= path.vectorPath.Count) return;

        float distanceToPlayer = Vector2.Distance(rb.position, target.position);

        if (distanceToPlayer <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * 140f * Time.fixedDeltaTime;
        rb.linearVelocity = force;

        if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            currentWaypoint++;
    }

    public void Move(Vector2 input) => Move();

    public Vector2 GetDir()
    {
        return ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
    }
}
