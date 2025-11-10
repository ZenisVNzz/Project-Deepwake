using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour, IEnemyController
{
    public IAIMove enemyMovement;
    public IState enemyState;
    public ICharacterDirectionHandler directionHandler;
    public IAnimationHandler animationHandler;
    public IStateHandler stateHandler;
    public IDamageDealer enemyAttack;

    private SpriteRenderer spriteRenderer;
    private Collider2D cd2D;
    private Collider2D hurtBox;

    public ICharacterRuntime enemyRuntime;

    private bool isDead = false;
    public bool IsDead => isDead;

    public void Init()
    {
        enemyMovement = GetComponent<IAIMove>();
        enemyState = GetComponent<IState>();
        directionHandler = GetComponent<ICharacterDirectionHandler>();
        animationHandler = GetComponent<IAnimationHandler>();
        stateHandler = GetComponent<IStateHandler>();
        enemyAttack = GetComponent<IDamageDealer>();

        enemyRuntime = GetComponent<ICharacterRuntime>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        cd2D = transform.Find("Collider").GetComponent<Collider2D>();
        hurtBox = transform.Find("HurtBox").GetComponent<Collider2D>();
        stateHandler.Register("OnDeath", OnDead);
    }

    private void OnAttack()
    {
        if (enemyMovement.HaveReachedTarget())
        {
            enemyAttack.Attack(enemyRuntime.TotalAttack);
        }      
    }

    private void OnMove()
    {
        if (enemyState.GetCurrentState() != CharacterStateType.Knockback && enemyState.GetCurrentState() != CharacterStateType.Death && enemyState.GetCurrentState() != CharacterStateType.Attacking)
        {
            enemyMovement.Move(enemyRuntime.TotalSpeed);
        }      
    }

    private void OnDead()
    {
        StartCoroutine(DeathProcess());
        isDead = true;
    }

    private IEnumerator DeathProcess()
    {
        cd2D.enabled = false;
        hurtBox.enabled = false;
        yield return new WaitForSeconds(3);
        spriteRenderer.DOFade(0f, 3f).OnComplete(() => Destroy(gameObject));
    }

    void Update()
    {
       
    }

    void FixedUpdate()
    {
        stateHandler.UpdateState();
        animationHandler.UpdateAnimation();
        OnMove();
        OnAttack();
    }
}
