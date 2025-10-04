using Mirror.BouncyCastle.Crypto.Signers;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IMovable playerMovement;
    private IState playerState;
    private ICharacterDirectionHandler directionHandler;
    private IAnimationHandler animationHandler;
    private IStateHandler stateHandler;
    private IDamageDealer playerAttack;

    private InputSystem_Actions inputHandler;

    public void Initialize
    (
      IMovable movement,
      IState state,
      IDamageDealer attack,
      IAnimationHandler animation,
      InputSystem_Actions input
    )
    {
        playerMovement = movement;
        playerState = state;
        playerAttack = attack;
        animationHandler = animation;
        inputHandler = input;

        inputHandler.Player.Attack.performed += ctx => OnAttack();
    }

    private void OnAttack()
    {
        playerAttack.Attack();
    }

    void Update()
    {
        stateHandler.UpdateState();
        animationHandler.UpdateAnimation();   
    }

    void FixedUpdate()
    {
        playerMovement.Move();
    }
}
