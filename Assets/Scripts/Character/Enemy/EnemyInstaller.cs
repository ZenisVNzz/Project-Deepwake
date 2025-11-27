using Pathfinding;
using UnityEngine;
using static EnemyFlyingMovement;

public class EnemyInstaller : CharacterInstaller
{
    [SerializeField] private bool isFlyingEnemy = false;

    private ICharacterRuntime _enemyRuntime;
    private IEnemyController _enemyController;
    private Seeker _seeker;

    public bool installOnEnemyShip = false;

    public override void GetComponent()
    {
        if (!isFlyingEnemy)
        {
            _seeker = GetComponent<Seeker>();
        }    

        _enemyRuntime = GetComponent<EnemyRuntime>();
        _enemyController = GetComponent<EnemyController>();
    }

    public override void InitComponent()
    {
        base.InitComponent();
        _enemyRuntime.Init();
        _enemyController.Init();
    }

    public override void InitCharacter()
    {
        GetComponent();
        InitComponent();

        if (installOnEnemyShip)
        {
            EnemyShipController.Instance.SetChild(this.transform, false, true);
        }
        else
        {
            ShipController.Instance.SetChild(this.transform, false, true);
        }
    }
}
