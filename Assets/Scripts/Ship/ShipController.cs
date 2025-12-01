using UnityEngine;

[RequireComponent(typeof(ObjectMove))]
public class ShipController : MonoBehaviour
{
    public static ShipController Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 5f; 
    [SerializeField] private float acceleration = 4f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float smoothStopDistance = 2.5f;
    [SerializeField] private GameObject background;

    [SerializeField] private Transform follower;

    private ObjectMove shipMover;
    private ObjectMove bgMover;

    private bool moving;
    private bool stopping;
    private bool movingToTarget;
    private bool continueAfterStop;
    private float targetStopX;

    public float currentSpeed => shipMover != null ? shipMover.Speed : 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        shipMover = GetOrAddComponent<ObjectMove>(gameObject);
        bgMover = background != null ? GetOrAddComponent<ObjectMove>(background) : null;

        if (bgMover == null && background != null)
            Debug.LogWarning("ShipController: Background assigned but has no ObjectMove.");
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

    private void Update()
    {
        if (moving) HandleAcceleration();
        if (movingToTarget) HandleMoveToTarget();
        if (stopping) HandleDeceleration();
    }

    private void HandleAcceleration()
    {
        float newSpeed = Mathf.MoveTowards(Mathf.Abs(shipMover.Speed), normalSpeed, acceleration * Time.deltaTime);
        SetSpeed(newSpeed);
    }

    private void HandleMoveToTarget()
    {
        float remainingX = targetStopX - transform.position.x;
        float absRemaining = Mathf.Abs(remainingX);

        if (absRemaining < smoothStopDistance)
        {
            float t = absRemaining / smoothStopDistance;

            float targetSpeed = continueAfterStop
                ? Mathf.Lerp(normalSpeed, maxSpeed, t)
                : Mathf.Lerp(0f, maxSpeed, t);

            float newSpeed = Mathf.MoveTowards(Mathf.Abs(shipMover.Speed), targetSpeed, deceleration * Time.deltaTime);
            SetSpeed(newSpeed);

            if (absRemaining < 0.05f)
            {
                if (continueAfterStop)
                {
                    continueAfterStop = false;
                    movingToTarget = false;
                    moving = true;
                }
                else
                {
                    StopImmediately();
                }
            }
        }
        else
        {
            float newSpeed = Mathf.MoveTowards(Mathf.Abs(shipMover.Speed), maxSpeed, acceleration * Time.deltaTime);
            SetSpeed(newSpeed);
        }
    }

    private void HandleDeceleration()
    {
        float remainingX = targetStopX - transform.position.x;
        float absRemaining = Mathf.Abs(remainingX);

        float newSpeed = Mathf.MoveTowards(Mathf.Abs(shipMover.Speed), 0f, deceleration * Time.deltaTime);
        SetSpeed(newSpeed);

        if (absRemaining < 0.05f || Mathf.Approximately(newSpeed, 0f))
            StopImmediately();
    }

    private void StopImmediately()
    {
        SetSpeed(0f);
        moving = false;
        stopping = false;
        movingToTarget = false;

        Vector3 pos = transform.position;
        pos.x = targetStopX;
        transform.position = pos;
    }

    public void StartMove()
    {
        moving = true;
        stopping = false;
        movingToTarget = false;
        continueAfterStop = false;
    }

    public void StopAtX(float x)
    {
        targetStopX = x;
        moving = false;
        movingToTarget = false;
        stopping = true;
        continueAfterStop = false;
    }

    public void MoveToX(float x, bool moveAfterStop)
    {
        targetStopX = x;
        moving = false;
        stopping = false;
        movingToTarget = true;
        continueAfterStop = moveAfterStop;
    }

    private void SetSpeed(float newSpeed)
    {
        if (movingToTarget || stopping)
        {
            float dir = Mathf.Sign(targetStopX - transform.position.x);
            shipMover.Speed = dir * Mathf.Abs(newSpeed);
        }
        else
        {
            shipMover.Speed = Mathf.Abs(newSpeed);
        }

        if (bgMover != null)
            bgMover.Speed = shipMover.Speed;
    }

    private T GetOrAddComponent<T>(GameObject obj) where T : Component =>
        obj.TryGetComponent(out T comp) ? comp : obj.AddComponent<T>();
}
