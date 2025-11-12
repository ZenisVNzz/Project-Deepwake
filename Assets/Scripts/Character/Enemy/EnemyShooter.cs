using System.Resources;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    private CharacterRuntime characterRuntime;

    private Transform player;

    private void Awake()
    {
        characterRuntime = GetComponent<CharacterRuntime>();

        if (firePoint == null)
        {
            var fp = new GameObject("FirePoint");
            fp.transform.SetParent(transform);
            fp.transform.localPosition = Vector3.zero;
            firePoint = fp.transform;
        }
    }

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (projectilePrefab == null)
        {
            projectilePrefab = ResourceManager.Instance?.GetAsset<GameObject>("Ball");
        }
    }

    public void Fire()
    {
        if (projectilePrefab == null)
            return;

        var go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        if (rb != null && player != null)
        {
            Vector2 direction = (player.position + Vector3.right * Random.Range(0.8f, 1.05f) - firePoint.position);
            direction = firePoint.InverseTransformDirection(direction); 
            direction.Normalize();
            rb.linearVelocity = firePoint.TransformDirection(direction) * 2.2f;
        }

        var proj = go.GetComponent<HitBoxHandler>();
        if (proj == null)
        {
            proj = go.AddComponent<HitBoxHandler>();
        }
        proj.SetData(characterRuntime.TotalAttack, gameObject.tag, characterRuntime);
    }
}
