using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerController : MonoBehaviour, IPlayerController
{
    private IMovable playerMovement;
    private IDashable playerDash;
    private IState playerState;
    private ICharacterDirectionHandler directionHandler;
    private IAnimationHandler animationHandler;
    private IStateHandler stateHandler;
    private IDamageDealer playerAttack;

    private InputSystem_Actions inputHandler;
    private IInteractionHandler interactionHandler;
    private IPlayerRuntime playerRuntime;

    private SpriteRenderer spriteRenderer;
    private Collider2D cd2D;
    private Collider2D hurtBox;

    private PlayerModifier playerModifier;
    public PlayerModifier PlayerModifier => playerModifier;

    private bool isMoveOnSlope = false;

    private CharacterUIManager _uiManager;

    public void Initialize
    (
      IMovable movement,
      IDashable dash,
      IState state,
      ICharacterDirectionHandler directionHandler,
      IDamageDealer attack,
      IAnimationHandler animation,
      IStateHandler stateHandler,
      InputSystem_Actions input,
      IPlayerRuntime playerRuntime
    )
    {
        this.playerMovement = movement;
        this.playerDash = dash;
        this.playerState = state;
        this.directionHandler = directionHandler;
        this.playerAttack = attack;
        this.animationHandler = animation;
        this.stateHandler = stateHandler;
        this.inputHandler = input;
        this.playerRuntime = playerRuntime;

        inputHandler.Player.Enable();
        inputHandler.Player.Attack.performed += ctx => OnAttack();
        inputHandler.Player.Move.performed += OnMove;
        inputHandler.Player.Move.canceled += OnMove;
        inputHandler.Player.Dash.performed += ctx => OnDash();
        inputHandler.Player.Interact.performed += ctx => OnInteract();
        inputHandler.Player.OpenInventory.performed += ctx => OnOpenCharMenu();
        inputHandler.Player.OpenOptions.performed += ctx => OnOpenGameMenu();

        interactionHandler = GetComponentInChildren<InteractionHandler>();
        playerModifier = new PlayerModifier(directionHandler);

        spriteRenderer = GetComponent<SpriteRenderer>();
        cd2D = transform.Find("Collider").GetComponent<Collider2D>();
        hurtBox = transform.Find("HurtBox").GetComponent<Collider2D>();
        stateHandler.Register("OnDeath", OnDead);

        _uiManager = FindAnyObjectByType<CharacterUIManager>();
    }

    private void OnInteract()
    {
        interactionHandler.Interact();    
    }

    private void OnOpenCharMenu()
    {
        if (_uiManager != null)
        {
            _uiManager.ToggleCharacterMenu();
        }
    }

    private void OnOpenGameMenu()
    {
        if (_uiManager != null)
        {
            _uiManager.ToggleOptionsMenu();
        }
    }

    private void OnAttack()
    {
        if (playerModifier.CanAttack)
        {
            playerAttack.Attack(playerRuntime.TotalAttack);
        }   
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (!playerModifier.CanMove)
        {
            return;
        }

        playerInput = context.ReadValue<Vector2>();
    }    

    private void OnDash()
    {
        if (playerModifier.CanDash)
        {
            playerDash.Dash();
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
        yield return null;
    }

    void Update()
    {       
        animationHandler.UpdateAnimation();   
    }

    public void MoveOnSlope(bool moveOnSlope)
    {
        isMoveOnSlope = moveOnSlope;
    }

    Vector2 playerInput;
    void FixedUpdate()
    {
        if (!playerModifier.CanMove) playerInput = Vector2.zero;

        playerMovement.Move(playerInput, playerRuntime.TotalSpeed, isMoveOnSlope);
        stateHandler.UpdateState();
    }
}
