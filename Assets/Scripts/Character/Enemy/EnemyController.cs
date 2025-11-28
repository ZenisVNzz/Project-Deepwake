using DG.Tweening;
using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : NetworkBehaviour, IEnemyController
{
    public IAIMove enemyMovement
    {
        get
        {
            return GetComponent<IAIMove>();
        }
    }

    private IState _enemyState;
    public IState enemyState
    {
        get
        {
            if (_enemyState == null)
                _enemyState = new EnemyState();
            return _enemyState;
        }
        set => _enemyState = value;
    }

    public ICharacterDirectionHandler directionHandler
    {
        get
        {
            return GetComponent<ICharacterDirectionHandler>();
        }
    }

    public IAnimationHandler animationHandler
    {
        get
        {
            return GetComponent<IAnimationHandler>();
        }
    }

    public IStateHandler stateHandler
    {
         get
        {
            return GetComponent<IStateHandler>();
        }
    }

    public IDamageDealer enemyAttack
    {
        get
        {
            return GetComponent<IDamageDealer>();
        }
    }

    public ICharacterRuntime enemyRuntime
    {
        get
        {
            return GetComponent<ICharacterRuntime>();
        }
    }

    protected SpriteRenderer spriteRenderer;
    protected Collider2D cd2D;
    protected Collider2D hurtBox;

    protected bool isDead = false;
    public bool IsDead => isDead;

    public void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        cd2D = transform.Find("Collider").GetComponent<Collider2D>();
        hurtBox = transform.Find("HurtBox").GetComponent<Collider2D>();
        stateHandler.Register("OnDeath", OnDead);
    }

    [Server]
    private void OnAttack()
    {
        if (enemyMovement.HaveReachedTarget())
        {
            enemyAttack.CmdAttack(enemyRuntime.TotalAttack);
        }      
    }

    [Server]
    private void OnMove()
    {
        if (enemyState.GetCurrentState() != CharacterStateType.Knockback && enemyState.GetCurrentState() != CharacterStateType.Death && enemyState.GetCurrentState() != CharacterStateType.Attacking)
        {
            enemyMovement.Move(enemyRuntime.TotalSpeed);
        }      
    }

    [Server]
    public virtual void OnDead()
    {
        if (isDead) return;
        isDead = true;

        RpcDeathEffect();
        StartCoroutine(ServerDeathProcess());
    }

    [ClientRpc]
    public virtual void RpcDeathEffect()
    {
        StartCoroutine(ClientDeathCoroutine());
    }

    public virtual IEnumerator ClientDeathCoroutine()
    {
        cd2D.enabled = false;
        hurtBox.enabled = false;

        yield return new WaitForSeconds(3f);

        spriteRenderer.DOFade(0f, 3f);
    }

    public virtual IEnumerator ServerDeathProcess()
    {
        yield return new WaitForSeconds(6f);

        if (NetworkServer.active)
            NetworkServer.Destroy(gameObject);
    }

    void Update()
    {
       
    }

    void FixedUpdate()
    {
        if (!isServer) return;

        stateHandler.UpdateState();
        animationHandler.UpdateAnimation();
        OnMove();
        OnAttack();
    }
}
