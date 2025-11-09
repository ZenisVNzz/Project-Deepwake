using Mirror;
using UnityEngine;

public class CharacterInstaller : NetworkBehaviour
{
    [SerializeField] protected CharacterData _characterData;

    protected IMovable _characterMovement;
    protected IAIMove _AIMovement;
    private IDashable _characterDash;
    protected IState _characterState;
    protected ICharacterDirectionHandler _directionHandler;
    protected IAnimationHandler _animationHandler;
    protected IStateHandler _stateHandler;
    protected IDamageDealer _characterAttack;

    protected IPlayerController _characterController;
    protected IPlayerRuntime _characterRuntime;

    protected Rigidbody2D _rigidbody2D;
    protected Collider2D _hurtBox;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    protected HitBoxController _hitBoxController;
    protected HitBoxController _skillHitBoxController;
    private InputSystem_Actions _inputHandler;

    protected CharacterData CharacterDataClone;

    public void SetData(CharacterData data)
    {
        _characterData = data;
    }

    protected void Awake()
    {
    }

    public virtual void GetComponent()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _hurtBox = gameObject.transform.Find("HurtBox").GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        if (gameObject.transform.Find("HitBoxs_BaseAttack") != null)
            _hitBoxController = gameObject.transform.Find("HitBoxs_BaseAttack").GetComponent<HitBoxController>();

        if (gameObject.transform.Find("HitBoxs_Skill") != null)
            _skillHitBoxController = gameObject.transform.Find("HitBoxs_Skill").GetComponent<HitBoxController>();

        CharacterDataClone = Instantiate(_characterData);
    }

    public virtual void InitComponent()
    {
        _characterRuntime = GetComponent<PlayerRuntime>();
        _inputHandler = new InputSystem_Actions();
        _characterState = new PlayerState();
        _characterMovement = new PlayerMovement(_rigidbody2D, _characterState);
        _characterDash = new PlayerDash(_rigidbody2D, _characterRuntime);
        _directionHandler = new PlayerDirectionHandler(_characterMovement);
        _animationHandler = new PlayerAnimationHandler(_animator, _characterState, _directionHandler);
        _stateHandler = new PlayerStateHandler(_characterState, _rigidbody2D, _inputHandler);
        _characterAttack = new PlayerAttack( _characterState, _hitBoxController);    
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log($"[CharacterInstaller] OnStartLocalPlayer for {gameObject.name}");
        InitCharacter();
    }

    public override void OnStartClient()
    {
        if (!isLocalPlayer)
        {
            Debug.Log($"[CharacterInstaller] OnStartClient for remote player {gameObject.name}");
            InitCharacter();
        }
    }

    public virtual void InitCharacter()
    {
        GetComponent();
        InitComponent();
        Inventory playerInventory = new Inventory();
        _characterRuntime.Init(CharacterDataClone, _rigidbody2D, _characterState, playerInventory);
        _characterController = GetComponent<PlayerController>();
        _characterController.Initialize
            (_characterMovement, _characterDash, _characterState, _directionHandler, _characterAttack, _animationHandler, _stateHandler, _inputHandler, _characterRuntime);

        var uiManager = FindAnyObjectByType<CharacterUIManager>();

        if (isLocalPlayer)
        {
            if (uiManager != null)
            {
                uiManager.Init(_characterRuntime);
            }

            CameraController.Instance.SetTarget(this.transform);
        }   

        if (_hitBoxController != null)
        {
            _hitBoxController.Init(_characterRuntime);
        }

        ShipController.Instance.SetChild(this.transform, false);
    }
}
