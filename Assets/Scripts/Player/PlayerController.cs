using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IMovable playerMovement;
    private IState playerState;
    private PlayerDirectionHander directionHander;
    private PlayerAnimationHandler animationHandler;
    private PlayerStateHandler stateHandler;

    private IDamageDealer playerAttack;

    private InputSystem_Actions inputActions;

    private Rigidbody2D rb;
    private Animator animator;
    private Joystick joystick;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        joystick = FindAnyObjectByType<Joystick>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerState = new PlayerState();
        playerMovement = new PlayerMovement(rb, joystick);
        directionHander = new PlayerDirectionHander(playerMovement);
        stateHandler = new PlayerStateHandler(playerState, playerMovement);
        animationHandler = new PlayerAnimationHandler(animator, playerState, directionHander);

        playerAttack = new PlayerAttack(playerState);

        inputActions.Player.Attack.performed += ctx => OnAttack();
    }

    void Start()
    {
           
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
