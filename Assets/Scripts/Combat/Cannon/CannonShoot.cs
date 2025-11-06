using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void Shoot()
    {
        CameraShake.Instance.ShakeCamera();
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        ICharacterRuntime characterRuntime = cannonController.CurPlayer.GetComponent<ICharacterRuntime>();
        CannonDamageCal cannonDamageCal = new CannonDamageCal();
        bullet.GetComponent<HitBoxHandler>().SetData(cannonDamageCal.Calculate(characterRuntime), "Player", characterRuntime);
        bullet.transform.position = spawnPos.position;
        bullet.AddComponent<CannonBulletRuntime>().Init(cannonNavigation.GetFireDirection());
    }  
}