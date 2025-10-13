using DG.Tweening;
using Mirror.BouncyCastle.Crypto.Signers;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
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
    private CharacterData characterData;

    private SpriteRenderer spriteRenderer;
    private Collider2D cd2D;
    private Collider2D hurtBox;

    private PlayerModifier playerModifier;
    public PlayerModifier PlayerModifier => playerModifier;

    public void Initialize
    (
      IMovable movement,
      IDashable dash,
      IState state,
      IDamageDealer attack,
      IAnimationHandler animation,
      IStateHandler stateHandler,
      InputSystem_Actions input,
      CharacterData characterData
    )
    {
        this.playerMovement = movement;
        this.playerDash = dash;
        this.playerState = state;
        this.playerAttack = attack;
        this.animationHandler = animation;
        this.stateHandler = stateHandler;
        this.inputHandler = input;
        this.characterData = characterData;

        inputHandler.Player.Enable();
        inputHandler.Player.Attack.performed += ctx => OnAttack();
        inputHandler.Player.Move.performed += OnMove;
        inputHandler.Player.Move.canceled += OnMove;
        inputHandler.Player.Dash.performed += ctx => OnDash();
        inputHandler.Player.Interact.performed += ctx => OnInteract();

        interactionHandler = GetComponentInChildren<InteractionHandler>();
        playerModifier = new PlayerModifier();

        spriteRenderer = GetComponent<SpriteRenderer>();
        cd2D = transform.Find("Collider").GetComponent<Collider2D>();
        hurtBox = transform.Find("HurtBox").GetComponent<Collider2D>();
        stateHandler.Register("OnDeath", OnDead);

    }

    private void OnInteract()
    {
        interactionHandler.Interact();    
    }

    private void OnAttack()
    {
        playerAttack.Attack(characterData.AttackPower);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (playerModifier.CanMove)
        {
            playerInput = context.ReadValue<Vector2>();
        }
        else
        {
            playerInput = Vector2.zero;
        }
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

    Vector2 playerInput;
    void FixedUpdate()
    {
        playerMovement.Move(playerInput, characterData.MoveSpeed);
        stateHandler.UpdateState();
    }
}
