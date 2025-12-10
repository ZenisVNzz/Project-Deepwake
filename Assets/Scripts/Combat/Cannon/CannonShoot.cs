using Mirror;
using UnityEngine;

public class CannonShoot
{
    private GameObject bulletPrefab;
    private CannonNavigation cannonNavigation;
    private Transform spawnPos;
    //
    private SFXData cannonFireSFX;
    //
    private CannonController cannonController;

    public CannonShoot(CannonNavigation cannonNavigation, Transform spawnPos, CannonController cannonController)
    {
        this.cannonNavigation = cannonNavigation;
        this.spawnPos = spawnPos;
        this.cannonController = cannonController;

        bulletPrefab = ResourceManager.Instance.GetAsset<GameObject>("Ball");
        cannonFireSFX = ResourceManager.Instance.GetAsset<SFXData>("CannonFireSFX");
    }

    [Server]
    public void Shoot(NetworkConnectionToClient Client)
    {
        SFXManager.Instance.Play(cannonFireSFX, spawnPos.position);
        GameObject bullet = GameObject.Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
        CharacterRuntime characterRuntime = cannonController.CurPlayer.GetComponent<CharacterRuntime>();
        CannonDamageCal cannonDamageCal = new CannonDamageCal();
        bullet.GetComponent<HitBoxHandler>().SetData(cannonDamageCal.Calculate(characterRuntime), "Player", characterRuntime);
        bullet.GetComponent<CannonBulletRuntime>().Init(cannonNavigation.GetFireDirection());
        NetworkServer.Spawn(bullet, Client);
        
    }
}