using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(IMovable))]
[RequireComponent(typeof(IDashable))]
[RequireComponent(typeof(ICharacterDirectionHandler))]
[RequireComponent(typeof(IAnimationHandler))]
[RequireComponent(typeof(IStateHandler))]
[RequireComponent(typeof(IDamageDealer))]
public class PlayerController : NetworkBehaviour, IPlayerController
{
    public IMovable playerMovement
    {
        get
        {
            return GetComponent<IMovable>();
        }
    }

    public IDashable playerDash
    {
        get
        {
            return GetComponent<IDashable>();
        }
    }

    private IState _playerState;
    public IState playerState
    {
        get
        {
            if (_playerState == null)
                _playerState = new PlayerState();
            return _playerState;
        }
        set => _playerState = value;
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

    public IDamageDealer playerAttack
    {
        get
        {
            return GetComponent<IDamageDealer>();
        }
    }

    public IPlayerRuntime playerRuntime
    {
        get
        {
            return GetComponent<IPlayerRuntime>();
        }
    }

    private InputSystem_Actions inputHandler;
    public InputSystem_Actions InputHandler
    {
        get
        {
            if (inputHandler == null)
            {
                inputHandler = new InputSystem_Actions();
            }
            return inputHandler;
        }
        set => inputHandler = value;
    }

    private PlayerModifier _playerModifier;
    public PlayerModifier playerModifier
    {
        get
        {
            if (_playerModifier == null)
            {
                _playerModifier = new PlayerModifier(directionHandler);
            }
            return _playerModifier;
        }
        set => _playerModifier = value;
    }

    Vector2 playerInput = new Vector2();

    private IInteractionHandler interactionHandler;

    private SpriteRenderer spriteRenderer;
    private Collider2D cd2D;
    private Collider2D hurtBox;

    private bool isMoveOnSlope = false;

    private CharacterUIManager _uiManager;

    private bool isDead = false;
    public bool IsDead => isDead;

    public void Init()
    {
        interactionHandler = GetComponentInChildren<InteractionHandler>();    

        spriteRenderer = GetComponent<SpriteRenderer>();
        cd2D = transform.Find("Collider").GetComponent<Collider2D>();
        hurtBox = transform.Find("HurtBox").GetComponent<Collider2D>();
        stateHandler.Register("OnDeath", OnDead);

        _uiManager = FindAnyObjectByType<CharacterUIManager>();

        ShipController.Instance.SetChild(this.transform, false);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        playerModifier = new PlayerModifier(directionHandler);

        InputHandler.Player.Enable();

        InputHandler.Player.Attack.performed += ctx => OnAttack();
        InputHandler.Player.Move.performed += OnMove;
        InputHandler.Player.Move.canceled += OnMove;
        InputHandler.Player.Dash.performed += ctx => OnDash();
        InputHandler.Player.Interact.performed += ctx => OnInteract();
        InputHandler.Player.OpenInventory.performed += ctx => OnOpenCharMenu();
        InputHandler.Player.OpenOptions.performed += ctx => OnOpenGameMenu();
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
        CmdAttack();
    }

    [Command]
    private void CmdAttack()
    {
        if (!playerModifier.CanAttack) return;
        playerAttack.Attack(playerRuntime.TotalAttack);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (!playerModifier.CanMove) return;

        Vector2 clientInput = context.ReadValue<Vector2>();
        CmdMove(clientInput);
    }

    [Command]
    private void CmdMove(Vector2 input)
    {
        playerInput = input;
    }

    private void OnDash()
    {
        CmdDash();
    }

    [Command]
    private void CmdDash()
    {
        if (!playerModifier.CanDash) return;

        playerDash.Dash();
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
        yield return null;
    }

    public void MoveOnSlope(bool moveOnSlope)
    {
        isMoveOnSlope = moveOnSlope;
    }

    [ServerCallback]
    void Update()
    {
        animationHandler.UpdateAnimation();
    }  

    [ServerCallback]
    void FixedUpdate()
    {
        if (!playerModifier.CanMove) playerInput = Vector2.zero;
        playerMovement.Move(playerInput, playerRuntime.TotalSpeed, isMoveOnSlope);

        stateHandler.UpdateState();
    }
}
