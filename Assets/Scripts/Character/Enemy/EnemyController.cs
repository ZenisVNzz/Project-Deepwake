using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour, IEnemyController
{
    private IAIMove enemyMovement;
    private IState enemyState;
    private ICharacterDirectionHandler directionHandler;
    private IAnimationHandler animationHandler;
    private IStateHandler stateHandler;
    private IDamageDealer enemyAttack;

    private SpriteRenderer spriteRenderer;
    private Collider2D cd2D;
    private Collider2D hurtBox;

    private CharacterData characterData;

    public void Initialize
    (
      IAIMove movement,
      IState state,
      IDamageDealer attack,
      IAnimationHandler animation,
      IStateHandler stateHandler,
      CharacterData characterData
    )
    {
        this.enemyMovement = movement;
        this.enemyState = state;
        this.enemyAttack = attack;
        this.animationHandler = animation;
        this.stateHandler = stateHandler;
        this.characterData = characterData;

        spriteRenderer = GetComponent<SpriteRenderer>();
        cd2D = transform.Find("Collider").GetComponent<Collider2D>();
        hurtBox = transform.Find("HurtBox").GetComponent<Collider2D>();
        stateHandler.Register("OnDeath", OnDead);
    }   

    private void OnAttack()
    {
        if (enemyMovement.HaveReachedTarget())
        {
            enemyAttack.Attack(characterData.AttackPower);
        }      
    }

    private void OnMove()
    {
        if (enemyState.GetCurrentState() != CharacterStateType.Knockback && enemyState.GetCurrentState() != CharacterStateType.Death && enemyState.GetCurrentState() != CharacterStateType.Attacking)
        {
            enemyMovement.Move(characterData.MoveSpeed);
        }      
    }

    private void OnDead()
    {
        StartCoroutine(DeathProcess());
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
