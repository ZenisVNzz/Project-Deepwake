using Pathfinding;
using UnityEngine;

public class EnemyInstaller : CharacterInstaller
{
    private ICharacterRuntime _enemyRuntime;
    private ICharacterController _enemyController;
    private Seeker _seeker;

    public override void GetComponent()
    {
        base.GetComponent();
        _seeker = GetComponent<Seeker>();
    }

    public override void InitComponent()
    {
        _enemyRuntime = gameObject.AddComponent<EnemyRuntime>();  
        _characterState = new EnemyState();
        _characterMovement = new EnemyMovement(_seeker, _rigidbody2D, _characterState, this);
        _directionHandler = new EnemyDirectionHandler(_characterMovement);
        _animationHandler = new EnemyAnimationHandler(_animator, _characterState, _directionHandler);
        _stateHandler = new EnemyStateHandler(_characterState, _rigidbody2D);
        _characterAttack = new EnemyAttack(_characterState, _hitBoxController);
    }

    public override void InitCharacter()
    {
        GetComponent();
        InitComponent();
        _enemyRuntime.Init(_characterData, _rigidbody2D, _characterState);
        _enemyController = gameObject.AddComponent<EnemyController>();
        _enemyController.Initialize(_characterMovement, _characterState, _characterAttack, _animationHandler, _stateHandler);
    }
}
