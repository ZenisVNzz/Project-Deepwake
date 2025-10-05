using Mirror.BouncyCastle.Crypto.Signers;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerController : MonoBehaviour, IPlayerController
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
      IStateHandler stateHandler,
      InputSystem_Actions input
    )
    {
        this.playerMovement = movement;
        this.playerState = state;
        this.playerAttack = attack;
        this.animationHandler = animation;
        this.stateHandler = stateHandler;
        this.inputHandler = input;

        inputHandler.Player.Enable();
        inputHandler.Player.Attack.performed += ctx => OnAttack();
        inputHandler.Player.Move.performed += OnMove;
        inputHandler.Player.Move.canceled += OnMove;
    }

    private void OnAttack()
    {
        playerAttack.Attack();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }    

    void Update()
    {
        stateHandler.UpdateState();
        animationHandler.UpdateAnimation();   
    }

    Vector2 playerInput;
    void FixedUpdate()
    {
        playerMovement.Move(playerInput);
    }
}
