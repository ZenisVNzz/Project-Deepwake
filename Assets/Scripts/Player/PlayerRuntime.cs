using UnityEngine;

[System.Serializable]
public class PlayerRuntime : MonoBehaviour, ICharacterRuntime
{
    [SerializeField] private float _hp;
    [SerializeField] private float _stamina;
    private PlayerData _playerData;
    private Rigidbody2D _rigidbody2D;

    public void Init(PlayerData playerData, Rigidbody2D rigidbody2D)
    {
        _hp = playerData.HP;
        _stamina = playerData.Stamina;
        _playerData = playerData;
        _rigidbody2D = rigidbody2D;
    }

    public void TakeDamage(float damage, Vector3 knockback)
    {
        if (this == null) return;
        _hp -= damage;
        _rigidbody2D.AddForce(knockback, ForceMode2D.Impulse);

        if (_hp <= 0)
        {
            Die();
        }

        Debug.Log($"Player took {damage} damage, remaining HP: {_hp}");
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
