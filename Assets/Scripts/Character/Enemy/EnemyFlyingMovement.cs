using UnityEngine;

public class EnemyFlyingMovement : IAIMove
{
    public enum VerticalSide { Above, Below }
    public enum AttackStyle { Melee, Ranged }

    private Transform target;
    private readonly Rigidbody2D rb;

    private readonly float chaseDistance = 30f; 
    private readonly float stopDistance = 2.2f; 
    private readonly float verticalSnapThreshold = 0.15f;

    private readonly float acceleration = 16f; 
    private readonly float maxTurnForce = 25f;
    private readonly float damping = 2.5f; 

    private readonly float horizSwayAmplitude = 1.8f; 
    private readonly float horizSwayFrequency = 1.2f; 
    private readonly float vertSwayAmplitude = 0.25f;
    private readonly float vertSwayFrequency = 0.9f;
    private readonly float swayPhase;

    private readonly float desiredVerticalOffset;
    private readonly VerticalSide keepSide; 
    private readonly AttackStyle attackStyle;

    private readonly float rangedMin = 4f;
    private readonly float rangedMax = 7f;

    private bool haveReachedTarget;

    public EnemyFlyingMovement(Rigidbody2D rb, MonoBehaviour runner)
    : this(rb, runner, AttackStyle.Ranged, VerticalSide.Above, 2.5f, 4f, 7f)
    { }

    public EnemyFlyingMovement(Rigidbody2D rb, MonoBehaviour runner, AttackStyle style, VerticalSide side,
    float verticalOffset = 2.5f, float rangedMin = 4f, float rangedMax = 7f)
    {
        this.rb = rb;
        this.attackStyle = style;
        this.keepSide = side;
        this.desiredVerticalOffset = Mathf.Abs(verticalOffset);
        this.rangedMin = Mathf.Max(0.1f, rangedMin);
        this.rangedMax = Mathf.Max(this.rangedMin + 0.1f, rangedMax);

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
        float desiredXCenter;
        if (attackStyle == AttackStyle.Melee)
        {
            desiredXCenter = target.position.x;
        }
        else
        {
            float ideal = (rangedMin + rangedMax) * 0.5f;
            float side = Mathf.Abs(dx) < 0.001f ? 1f : Mathf.Sign(dx); 
            desiredXCenter = target.position.x + side * ideal;
        }
        float swayX = Mathf.Sin(Time.time * horizSwayFrequency + swayPhase) * horizSwayAmplitude;
        float desiredX = desiredXCenter + swayX;

        Vector2 desiredPos = new Vector2(desiredX, desiredY);
        Vector2 toTarget = desiredPos - rb.position;

        if (distToPlayer <= stopDistance)
        {
            haveReachedTarget = true;
        }
        else
        {
            haveReachedTarget = false;
        }

        Vector2 curVel = rb.linearVelocity;
        Vector2 desiredVel = toTarget.normalized * moveSpeed;
        Vector2 steer = Vector2.ClampMagnitude(desiredVel - curVel, maxTurnForce);
        Vector2 newVel = curVel + steer * Mathf.Clamp(acceleration * Time.deltaTime, 0f, 1f);
        newVel = Vector2.Lerp(newVel, Vector2.ClampMagnitude(newVel, moveSpeed), 1f - Mathf.Exp(-damping * Time.deltaTime));

        rb.linearVelocity = newVel;
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
