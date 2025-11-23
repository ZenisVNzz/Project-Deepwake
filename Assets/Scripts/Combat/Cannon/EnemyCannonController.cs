using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonController : NetworkBehaviour
{
    private NetworkIdentity currentEnemy;
    public NetworkIdentity CurEnemy => currentEnemy;

    public PlayerRuntime Target;

    [SerializeField] Transform ownerLockPos;
    [SerializeField] Direction ownerLockDir;
    [SerializeField] GameObject RotateObj;
    [SerializeField] GameObject NavigateGuideObj;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform recoilPivot;
    [SerializeField] bool isFront;

    private Animator animator;
    private Vector2 input;

    private CannonNavigation cannonNavigation;
    private EnemyCannonShoot cannonShoot;

    private float cooldown = 1f;
    private float timer;

    private void Awake()
    {
        cannonNavigation = new CannonNavigation(RotateObj, NavigateGuideObj, recoilPivot, isFront);
        cannonShoot = new EnemyCannonShoot(cannonNavigation, shootPos, this);
        animator = GetComponent<Animator>();

        timer = cooldown;
    }

    private void Start()
    {
        Target = GetRadomTarget();
    }

    private void Update()
    {
        if (!isServer) return;

        input = GetInputDirToPlayer();
        cannonNavigation.UpdateNavigation(input.x);

        if (currentEnemy != null)
        {
            
        }

        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            if (Target != null && IsAimedAtTarget())
            {
                //cannonShoot.Shoot();
                //cannonNavigation.ApplyRecoil();
            }
            timer = 0f;
        }
    }

    [Server]
    private void UseCannon(NetworkIdentity networkIdentity)
    {
        if (currentEnemy != null) return;
        currentEnemy = networkIdentity;

        GameObject ownerObj = networkIdentity.gameObject;
        ownerObj.transform.position = ownerLockPos.position;
        GhostPirateMovement movement = ownerObj.GetComponent<GhostPirateMovement>();
        movement.CanMove = false;
    }

    [Server]
    public void ReleaseCannon()
    {
        if (currentEnemy == null) return;
        GameObject ownerObj = currentEnemy.gameObject;
        GhostPirateMovement movement = ownerObj.GetComponent<GhostPirateMovement>();
        movement.CanMove = true;
        currentEnemy = null;
    }

    private PlayerRuntime GetRadomTarget()
    {
        List<PlayerRuntime> players =  PlayerNetManager.Instance.GetAllPlayerRuntimes();
        if (players.Count == 0) return null;
        int index = Random.Range(0, players.Count);
        return players[index];
    }

    private Vector2 GetInputDirToPlayer()
    {
        if (Target == null || RotateObj == null || shootPos == null)
            return Vector2.zero;

        Vector3 toTarget = Target.transform.position - shootPos.position;
        toTarget.z = 0f;
        if (toTarget.sqrMagnitude < 0.0001f)
            return Vector2.zero;

        Vector3 currentFireDir = RotateObj.transform.up; 
        Vector2 from = new Vector2(currentFireDir.x, currentFireDir.y).normalized;
        Vector2 to = new Vector2(toTarget.x, toTarget.y).normalized;

        float angleDiff = Vector2.SignedAngle(from, to); // degrees

        const float sensitivity = 20f;
        float inputX = Mathf.Clamp(angleDiff / sensitivity, -1f, 1f);

        return new Vector2(inputX, 0f);
    }

    private bool IsAimedAtTarget()
    {
        if (Target == null || RotateObj == null || shootPos == null)
            return false;

        Vector3 toTarget = Target.transform.position - shootPos.position;
        toTarget.z = 0f;
        if (toTarget.sqrMagnitude < 0.0001f)
            return false;

        Vector3 currentFireDir = RotateObj.transform.up;
        Vector2 from = new Vector2(currentFireDir.x, currentFireDir.y).normalized;
        Vector2 to = new Vector2(toTarget.x, toTarget.y).normalized;

        float angleDiff = Mathf.Abs(Vector2.SignedAngle(from, to));
        const float aimTolerance = 8f;
        return angleDiff <= aimTolerance;
    }
}
