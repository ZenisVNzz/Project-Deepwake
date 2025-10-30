using UnityEngine;

public class EnemyFlyingMovement : IAIMove
{
    public enum VerticalSide { Above, Below }

    private Transform target;
    private readonly Rigidbody2D rb;

    private readonly float chaseDistance = 30f; 
    private readonly float stopDistance = 0.8f; 

    private readonly float acceleration = 16f; 
    private readonly float maxTurnForce = 25f;
    private readonly float damping = 2.5f; 

    private readonly float horizSwayAmplitude = 9f; 
    private readonly float horizSwayFrequency = 1.8f; 
    private readonly float vertSwayAmplitude = 0.25f;
    private readonly float vertSwayFrequency = 0.9f;
    private readonly float swayPhase;

    private readonly float desiredVerticalOffset;
    private readonly VerticalSide keepSide; 

    private bool haveReachedTarget;

    public EnemyFlyingMovement(Rigidbody2D rb, MonoBehaviour runner)
    : this(rb, runner, VerticalSide.Above, 2.5f, 0.8f)
    { }

    public EnemyFlyingMovement(Rigidbody2D rb, MonoBehaviour runner, VerticalSide side,
    float verticalOffset = 2.5f, float stopDistance = 0.8f)
    {
        this.rb = rb;
        this.keepSide = side;
        this.desiredVerticalOffset = Mathf.Abs(verticalOffset);
        this.stopDistance = stopDistance;

        swayPhase = Random.value * Mathf.PI * 2f;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) target = playerObj.transform;
    }

    public void Move(float moveSpeed)
    {
        if (target == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj == null) return;
            target = playerObj.transform;
        }

        float distToPlayer = Vector2.Distance(rb.position, target.position);
        if (distToPlayer > chaseDistance)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 1f - Mathf.Exp(-damping * Time.deltaTime));
            haveReachedTarget = false;
            return;
        }

        float sideSign = keepSide == VerticalSide.Above ? 1f : -1f;
        float desiredLaneY = target.position.y + sideSign * desiredVerticalOffset;
        float swayY = Mathf.Sin(Time.time * vertSwayFrequency + swayPhase) * vertSwayAmplitude;
        float desiredY = desiredLaneY + swayY;

        float dx = target.position.x - rb.position.x;
        float desiredXCenter = target.position.x;

        float swayX = Mathf.Sin(Time.time * horizSwayFrequency + swayPhase) * horizSwayAmplitude;
        float desiredX = desiredXCenter + swayX;

        Vector2 toPlayer = (Vector2)target.position - rb.position;    

        Vector2 desiredPos = (Vector2)target.position - toPlayer.normalized * stopDistance;
        desiredPos.y += (keepSide == VerticalSide.Above ? desiredVerticalOffset : -desiredVerticalOffset);
        desiredPos.x += Mathf.Sin(Time.time * horizSwayFrequency + swayPhase) * horizSwayAmplitude;
        desiredPos.y += Mathf.Sin(Time.time * vertSwayFrequency + swayPhase) * vertSwayAmplitude;

        Vector2 toTarget = desiredPos - rb.position;

        Vector2 curVel = rb.linearVelocity;
        Vector2 desiredVel = toTarget.normalized * moveSpeed;
        Vector2 steer = Vector2.ClampMagnitude(desiredVel - curVel, maxTurnForce);
        Vector2 newVel = curVel + steer * Mathf.Clamp(acceleration * Time.deltaTime, 0f, 1f);
        newVel = Vector2.Lerp(newVel, Vector2.ClampMagnitude(newVel, moveSpeed), 1f - Mathf.Exp(-damping * Time.deltaTime));

        rb.linearVelocity = newVel;

        if (distToPlayer / 2.1f <= stopDistance)
        {
            haveReachedTarget = true;
        }
        else
        {
            haveReachedTarget = false;
        }
    }

    public void Move(Vector2 input, float moveSpeed, bool isMoveOnSlope) => Move(moveSpeed);

    public Vector2 GetDir()
    {
        Vector2 vel = rb.linearVelocity;
        if (vel.sqrMagnitude < 0.0001f)
        {
            if (target == null) return Vector2.right;
            float sideSign = keepSide == VerticalSide.Above ? 1f : -1f;
            Vector2 laneTarget = new Vector2(target.position.x, target.position.y + sideSign * desiredVerticalOffset);
            return (laneTarget - rb.position).normalized;
        }
        return vel.normalized;
    }

    public bool HaveReachedTarget() => haveReachedTarget;
}
