using Mirror;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(IMovable))]
[RequireComponent(typeof(IDashable))]
[RequireComponent(typeof(ICharacterDirectionHandler))]
[RequireComponent(typeof(IAnimationHandler))]
[RequireComponent(typeof(IStateHandler))]
[RequireComponent(typeof(IDamageDealer))]
public class PlayerController : NetworkBehaviour, IPlayerController
{
    public PlayerMovement playerMovement
    {
        get
        {
            return GetComponent<PlayerMovement>();
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

    public PlayerAnimationHandler animationHandler
    {
        get
        {
            return GetComponent<PlayerAnimationHandler>();
        }
    }

    public PlayerStateHandler stateHandler
    {
        get
        {
            return GetComponent<PlayerStateHandler>();
        }
    }

    public PlayerAttack playerAttack
    {
        get
        {
            return GetComponent<PlayerAttack>();
        }
    }

    public PlayerRuntime playerRuntime
    {
        get
        {
            return GetComponent<PlayerRuntime>();
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

    [SyncVar] Vector2 playerInput = new Vector2();

    private IInteractionHandler interactionHandler;

    private SpriteRenderer spriteRenderer;
    private Collider2D cd2D;
    private Collider2D hurtBox;

    private bool isMoveOnSlope = false;
    public bool reverseSlope = false;

    private CharacterUIManager _uiManager;

    private bool isDead = false;
    public bool IsDead => isDead;

    public event Action OnPlayerDead;

    public Button DebugButton;

    public void Init()
    {
        interactionHandler = GetComponentInChildren<InteractionHandler>();    

        spriteRenderer = GetComponent<SpriteRenderer>();
        cd2D = transform.Find("Collider").GetComponent<Collider2D>();
        hurtBox = transform.Find("HurtBox").GetComponent<Collider2D>();
        stateHandler.Register("OnDeath", OnDead);

        _uiManager = FindAnyObjectByType<CharacterUIManager>();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        InputHandler.Player.Enable();

        InputHandler.Player.Attack.performed += ctx => OnAttack();
        InputHandler.Player.Move.performed += OnMove;
        InputHandler.Player.Move.canceled += OnMove;
        InputHandler.Player.Dash.performed += ctx => OnDash();
        InputHandler.Player.Interact.performed += ctx => OnInteract();
        InputHandler.Player.OpenInventory.performed += ctx => OnOpenCharMenu();
        InputHandler.Player.OpenOptions.performed += ctx => OnOpenGameMenu();
        InputHandler.Player.OpenDebug.performed += ctx => DebugUI.Instance.ToggleDebugUI();
        DebugButton.onClick.AddListener(() => DebugUI.Instance.ToggleDebugUI());
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
        if (!isLocalPlayer || !playerModifier.CanAttack) return;
        playerAttack.CmdAttack(playerRuntime.TotalAttack);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (!playerModifier.CanMove || !isLocalPlayer) return;

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
        if (!isLocalPlayer) return;

        CmdDash();
    }

    [Command]
    private void CmdDash()
    {
        if (!playerModifier.CanDash) return;

        playerDash.Dash();
    }

    public void OnDead()
    {
        StartCoroutine(DeathProcess());
        isDead = true;
        OnPlayerDead?.Invoke();
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

    void Update()
    { 
        stateHandler.UpdateState();
        animationHandler.UpdateAnimation();
    }

    [Server]
    void FixedUpdate()
    {
        if (!playerModifier.CanMove) playerInput = Vector2.zero;
        playerMovement.CmdMove(playerInput, playerRuntime.TotalSpeed, isMoveOnSlope); 
    }
}
