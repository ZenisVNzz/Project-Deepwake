using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonShoot
{
    private GameObject bulletPrefab;
    private CannonNavigation cannonNavigation;
    private Transform spawnPos;

    public CannonShoot(CannonNavigation cannonNavigation, Transform spawnPos)
    {
        this.cannonNavigation = cannonNavigation;
        this.spawnPos = spawnPos;

        bulletPrefab = ResourceManager.Instance.GetAsset<GameObject>("Ball");
    }

    public void Shoot()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab);
        bullet.transform.position = spawnPos.position;
        bullet.AddComponent<CannonBulletRuntime>().Init(cannonNavigation.GetFireDirection());
    }  
}