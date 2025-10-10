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
    }

    private void OnAttack()
    {
        enemyAttack.Attack();
    }

    private void OnMove()
    {
        if (enemyState.GetCurrentState() != CharacterStateType.Knockback)
        {
            enemyMovement.Move();
        }      
    }

    void Update()
    {
        animationHandler.UpdateAnimation();
    }

    void FixedUpdate()
    {
        OnMove();
        stateHandler.UpdateState();
    }
}
