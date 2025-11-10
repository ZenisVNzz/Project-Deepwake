using Pathfinding;
using UnityEngine;
using static EnemyFlyingMovement;

public class EnemyInstaller : CharacterInstaller
{
    [SerializeField] private bool isFlyingEnemy = false;

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
    }

    public override void InitCharacter()
    {
        GetComponent();
        InitComponent();
        _enemyRuntime.Init();
        _enemyController.Init();

        ShipController.Instance.SetChild(this.transform, true);
    }
}
