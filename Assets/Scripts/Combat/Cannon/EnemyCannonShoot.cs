using Mirror;
using UnityEngine;

public class EnemyCannonShoot
{
    private GameObject bulletPrefab;
    private IEnemyCannonNavigation cannonNavigation;
    private Transform spawnPos;

    private EnemyCannonController cannonController;
    private SFXData SFXData => ResourceManager.Instance.GetAsset<SFXData>("CannonFireSFX");

    public EnemyCannonShoot(IEnemyCannonNavigation cannonNavigation, Transform spawnPos, EnemyCannonController cannonController)
    {
        this.cannonNavigation = cannonNavigation;
        this.spawnPos = spawnPos;
        this.cannonController = cannonController;

        bulletPrefab = ResourceManager.Instance.GetAsset<GameObject>("Ball");
    }

    [Server]
    public void Shoot()
    {
        SFXManager.Instance.Play(SFXData, spawnPos.position);
        GameObject bullet = GameObject.Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
        CharacterRuntime characterRuntime = cannonController.CurEnemy.GetComponent<CharacterRuntime>();
        CannonDamageCal cannonDamageCal = new CannonDamageCal();
        bullet.GetComponent<HitBoxHandler>().SetData(cannonDamageCal.Calculate(characterRuntime), "Enemy", characterRuntime);
        bullet.GetComponent<CannonBulletRuntime>().Init(cannonNavigation.GetFireDirection());
        NetworkServer.Spawn(bullet);
    }
}