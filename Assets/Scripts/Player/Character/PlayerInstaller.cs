using UnityEngine;

public class PlayerInstaller : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    private IMovable _playerMovement;
    private IState _playerState;
    private ICharacterDirectionHandler _directionHandler;
    private IAnimationHandler _animationHandler;
    private IStateHandler _stateHandler;
    private IDamageDealer _playerAttack;

    private IPlayerController _playerController;

    private Rigidbody2D _rigidbody2D;
    private Collider2D _hurtBox;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private HitBoxController _hitBoxController;
    private HitBoxController _skillHitBoxController;
    private Joystick _joystick;

    private void Awake()
    {
        InitPlayer();
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
        _joystick = FindAnyObjectByType<Joystick>();
    }

    public virtual void InitComponent()
    {      
        _playerState = new PlayerState();
        _playerMovement = new PlayerMovement(_rigidbody2D, _joystick, _playerState);
        _directionHandler = new PlayerDirectionHandler(_playerMovement);
        _animationHandler = new PlayerAnimationHandler(_animator, _playerState, _directionHandler);
        _stateHandler = new PlayerStateHandler(_playerState, _playerMovement);
        _playerAttack = new PlayerAttack( _playerState, _hitBoxController);
    }

    public virtual void InitPlayer()
    {
        GetComponent();
        InitComponent();
        ICharacterRuntime playerRuntime = gameObject.AddComponent<PlayerRuntime>();
        playerRuntime.Init(_playerData, _rigidbody2D);
        _playerController = gameObject.AddComponent<PlayerController>();
        _playerController.Initialize(_playerMovement, _playerState, _playerAttack, _animationHandler, _stateHandler, new InputSystem_Actions());
    }
}
