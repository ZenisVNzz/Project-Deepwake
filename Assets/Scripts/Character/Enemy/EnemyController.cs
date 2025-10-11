using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour, ICharacterController
{
    private IMovable enemyMovement;
    private IState enemyState;
    private ICharacterDirectionHandler directionHandler;
    private IAnimationHandler animationHandler;
    private IStateHandler stateHandler;
    private IDamageDealer enemyAttack;

    private SpriteRenderer spriteRenderer;
    private Collider2D cd2D;

    public void Initialize
    (
      IMovable movement,
      IState state,
      IDamageDealer attack,
      IAnimationHandler animation,
      IStateHandler stateHandler
    )
    {
        this.enemyMovement = movement;
        this.enemyState = state;
        this.enemyAttack = attack;
        this.animationHandler = animation;
        this.stateHandler = stateHandler;

        spriteRenderer = GetComponent<SpriteRenderer>();
        cd2D = transform.Find("Collider").GetComponent<Collider2D>();
        stateHandler.Register("OnDeath", OnDead);
    }

    private void OnAttack()
    {
        enemyAttack.Attack();
    }

    private void OnMove()
    {
        if (enemyState.GetCurrentState() != CharacterStateType.Knockback && enemyState.GetCurrentState() != CharacterStateType.Death)
        {
            enemyMovement.Move();
        }      
    }

    private void OnDead()
    {
        StartCoroutine(DeathProcess());
    }

    private IEnumerator DeathProcess()
    {
        cd2D.enabled = false;
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
    }
}
