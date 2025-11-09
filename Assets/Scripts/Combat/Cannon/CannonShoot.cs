using Mirror;
using UnityEngine;

public class CannonShoot
{
    private GameObject bulletPrefab;
    private CannonNavigation cannonNavigation;
    private Transform spawnPos;

    private CannonController cannonController;

    public CannonShoot(CannonNavigation cannonNavigation, Transform spawnPos, CannonController cannonController)
    {
        this.cannonNavigation = cannonNavigation;
        this.spawnPos = spawnPos;
        this.cannonController = cannonController;

        bulletPrefab = ResourceManager.Instance.GetAsset<GameObject>("Ball");
    }

    [Server]
    public void Shoot()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
        ICharacterRuntime characterRuntime = cannonController.CurPlayer.GetComponent<ICharacterRuntime>();
        CannonDamageCal cannonDamageCal = new CannonDamageCal();
        bullet.GetComponent<HitBoxHandler>().SetData(cannonDamageCal.Calculate(characterRuntime), "Player", characterRuntime);
        bullet.AddComponent<CannonBulletRuntime>().Init(cannonNavigation.GetFireDirection());
        NetworkServer.Spawn(bullet);
    }
}