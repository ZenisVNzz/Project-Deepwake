using Pathfinding;
using UnityEngine;
using static EnemyFlyingMovement;

public class EnemyInstaller : CharacterInstaller
{
    [SerializeField] private bool isFlyingEnemy = false;
    [SerializeField] private bool isTwoDirEnemy = false;

    private ICharacterRuntime _enemyRuntime;
    private IEnemyController _enemyController;
    private Seeker _seeker;

    public override void GetComponent()
    {
        base.GetComponent();
        if (!isFlyingEnemy)
        {
            _seeker = GetComponent<Seeker>();
        }    
    }

    public override void InitComponent()
    {
        _enemyRuntime = gameObject.AddComponent<EnemyRuntime>();
        _characterState = new EnemyState();
        _characterAttack = new EnemyAttack(_characterState, _hitBoxController);

        if (isFlyingEnemy)
            _AIMovement = new EnemyFlyingMovement(_rigidbody2D, this, AttackStyle.Melee, VerticalSide.Below);
        else
            _AIMovement = new EnemyMovement(_seeker, _rigidbody2D, this);

        if (isTwoDirEnemy)
        {
            _directionHandler = new EnemyDirectionHandler(_AIMovement, true);
            _animationHandler = new EnemyAnimationHandler(_animator, _characterState, _directionHandler, true);
        }
        else
        {
            _directionHandler = new EnemyDirectionHandler(_AIMovement);
            _animationHandler = new EnemyAnimationHandler(_animator, _characterState, _directionHandler);
        }
           
        _stateHandler = new EnemyStateHandler(_characterState, _rigidbody2D);   
    }

    public override void InitCharacter()
    {
        GetComponent();
        InitComponent();
        _enemyRuntime.Init(CharacterDataClone, _rigidbody2D, _characterState);
        _enemyController = gameObject.AddComponent<EnemyController>();
        _enemyController.Initialize(_AIMovement, _characterState, _characterAttack, _animationHandler, _stateHandler, CharacterDataClone);
    }
}
