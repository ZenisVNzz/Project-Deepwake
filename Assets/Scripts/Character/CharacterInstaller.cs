using UnityEngine;

public class CharacterInstaller : MonoBehaviour
{
    [SerializeField] protected CharacterData _characterData;

    protected IMovable _characterMovement;
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

    protected void Awake()
    {
        InitCharacter();
    }

    public virtual void GetComponent()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _hurtBox = gameObject.transform.Find("HurtBox").GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _hitBoxController = gameObject.transform.Find("HitBoxs_BaseAttack").GetComponent<HitBoxController>();

        if (gameObject.transform.Find("HitBoxs_Skill") != null)
        {
            _skillHitBoxController = gameObject.transform.Find("HitBoxs_Skill").GetComponent<HitBoxController>();
        }
    }

    public virtual void InitComponent()
    {
        _characterRuntime = gameObject.AddComponent<PlayerRuntime>();
        _inputHandler = new InputSystem_Actions();
        _characterState = new PlayerState();
        _characterMovement = new PlayerMovement(_rigidbody2D, _characterState, _characterData);
        _characterDash = new PlayerDash(_rigidbody2D, _characterRuntime);
        _directionHandler = new PlayerDirectionHandler(_characterMovement);
        _animationHandler = new PlayerAnimationHandler(_animator, _characterState, _directionHandler);
        _stateHandler = new PlayerStateHandler(_characterState, _rigidbody2D, _inputHandler);
        _characterAttack = new PlayerAttack( _characterState, _hitBoxController);
    }

    public virtual void InitCharacter()
    {
        GetComponent();
        InitComponent();
        _characterRuntime.Init(_characterData, _rigidbody2D, _characterState);
        _characterController = gameObject.AddComponent<PlayerController>();
        _characterController.Initialize(_characterMovement, _characterDash, _characterState, _characterAttack, _animationHandler, _stateHandler, _inputHandler);
    }
}
