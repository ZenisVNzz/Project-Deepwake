using Pathfinding;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyInstaller : CharacterInstaller
{
    [SerializeField] private bool isFlyingEnemy = false;

    private ICharacterRuntime _enemyRuntime;
    private IEnemyController _enemyController;
    private Seeker _seeker;

    public bool installOnEnemyShip = false;
    public bool hasAwakeAnimation = false;

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

    public override async void InitCharacter()
    {
        if (installOnEnemyShip)
        {
            EnemyShipController.Instance.SetChild(this.transform, true, false);
        }
        else
        {
            ShipController.Instance.SetChild(this.transform, true, false);
        }

        GetComponent();
        InitComponent();
    }
}
