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

    private EnemyCannonNavigation enemyCannonNavigation;
    private EnemyCannonShoot cannonShoot;

    private float cooldown = 3.5f;
    private float timer;

    private bool active = false;

    private void Awake()
    {
        enemyCannonNavigation = new EnemyCannonNavigation(RotateObj, NavigateGuideObj, recoilPivot, isFront);
        cannonShoot = new EnemyCannonShoot(enemyCannonNavigation, shootPos, this);
        animator = GetComponent<Animator>();

        timer = cooldown;
    }

    private void Start()
    {
        Target = GetRadomTarget();
    }

    private void Update()
    {
        if (!isServer || !active) return;

        timer += Time.deltaTime;

        if (Target != null)
        {
            enemyCannonNavigation.UpdateNavigationTowardsTarget(Target.transform.position);

            if (timer >= cooldown && CurEnemy != null)
            {
                timer = 0f;
                // shoot
                cannonShoot.Shoot();
                enemyCannonNavigation.ApplyRecoil();
            }
        }
        else
        {
            if (timer >= cooldown)
            {
                Target = GetRadomTarget();
                timer = 0f;
            }
        }
    }

    [Server]
    public void UseCannon(NetworkIdentity networkIdentity)
    {
        if (currentEnemy != null) return;
        currentEnemy = networkIdentity;

        GameObject ownerObj = networkIdentity.gameObject;
        ownerObj.transform.position = ownerLockPos.position;
        GhostPirateMovement movement = ownerObj.GetComponent<GhostPirateMovement>();
        movement.CanMove = false;

        CharacterRuntime characterRuntime = ownerObj.GetComponent<CharacterRuntime>();
        characterRuntime.OnHit += ReleaseCannon;
        active = true;
    }

    [Server]
    public void ReleaseCannon()
    {
        if (currentEnemy == null) return;
        GameObject ownerObj = currentEnemy.gameObject;
        GhostPirateMovement movement = ownerObj.GetComponent<GhostPirateMovement>();
        movement.CanMove = true;

        CharacterRuntime characterRuntime = ownerObj.GetComponent<CharacterRuntime>();
        characterRuntime.OnHit -= ReleaseCannon;

        currentEnemy = null;
        active = false;
    }

    private PlayerRuntime GetRadomTarget()
    {
        List<PlayerRuntime> players =  PlayerNetManager.Instance.GetAllPlayerRuntimes();
        if (players.Count == 0) return null;
        int index = Random.Range(0, players.Count);
        return players[index];
    }
}
