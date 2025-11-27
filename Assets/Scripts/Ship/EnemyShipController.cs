using UnityEngine;
using UnityEngine.UI;

public class EnemyShipController : MonoBehaviour, IAttackable
{
    public static EnemyShipController Instance { get; private set; }
    [SerializeField] private Transform follower;

    public Slider BossStatusUI;

    public float MaxHP = 3000f;
    public float HP = 3000f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        HP = MaxHP;
        BossStatusUI.maxValue = HP;
        BossStatusUI.value = HP;
    }

    public void SetChild(Transform child, bool worldPositionStay, bool ResetPostion)
    {
        if (follower == null) return;
        Vector3 originalScale = child.localScale;
        child.SetParent(follower, worldPositionStay);


        if (ResetPostion)
        {
            child.localPosition = Vector3.zero;
        }

        child.localScale = originalScale;
    }

    public void TakeDamage(float dmg, Vector3 knockback, ICharacterRuntime attacker)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            HP = 0;
        }

        BossStatusUI.value = HP;
    }

    public void TakeDamage(float dmg, Vector3 knockback) => TakeDamage(dmg, knockback, null);

    public void Repair(float amount)
    {
        HP += amount;
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
    }
}
