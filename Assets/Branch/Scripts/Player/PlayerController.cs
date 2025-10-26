using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IMovable playerMovement;
    private PlayerState playerState;
    private PlayerDirectionHander directionHander;
    private PlayerAnimationHandler animationHandler;
    private PlayerStateHandler stateHandler;

    private Rigidbody2D rb;
    private Animator animator;
    private Joystick joystick;

    void Start()
    {
        joystick = FindAnyObjectByType<Joystick>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerState = new PlayerState();
        playerMovement = new PlayerMovement(rb, joystick);
        directionHander = new PlayerDirectionHander(playerMovement);
        stateHandler = new PlayerStateHandler(playerState, playerMovement);
        animationHandler = new PlayerAnimationHandler(animator, playerState, directionHander);   
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
