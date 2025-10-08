using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRuntime : MonoBehaviour, IPlayerRuntime
{
    [SerializeField] private float _hp;
    [SerializeField] private float _stamina;
    public float HP => _hp;
    public float Stamina => _stamina;

    private float _hpRegenRate;
    private float _staminaRegenRate;
    private float _staminaConsumptionMultiplier;
    private Coroutine _staminaRegenCoroutine;

    private CharacterData _playerData;
    public CharacterData PlayerData => _playerData;

    private Rigidbody2D _rigidbody2D;

    public void Init(CharacterData playerData, Rigidbody2D rigidbody2D)
    {
        _playerData = Instantiate(playerData);
        _hp = playerData.HP;
        _stamina = playerData.Stamina;
        _hpRegenRate = playerData.HPRegenRate;
        _staminaRegenRate = playerData.StaminaRegenRate;
        _staminaConsumptionMultiplier = playerData.StaminaConsumptionMultiplier;

        _rigidbody2D = rigidbody2D;
    }

    public bool UseStamina(float amount)
    {
        float adjustedAmount = amount * _staminaConsumptionMultiplier;
        if (_stamina >= adjustedAmount)
        {
            _stamina -= adjustedAmount;

            if (_staminaRegenCoroutine != null)
            {
                StopCoroutine(_staminaRegenCoroutine);
            }
            _staminaRegenCoroutine = StartCoroutine(RegenSatamina());

            return true;
        }
        return false;
    }

    private IEnumerator RegenSatamina()
    {
        yield return new WaitForSeconds(2f);
        while (_stamina < _playerData.Stamina)
        {
            _stamina += _staminaRegenRate * Time.deltaTime;
            if (_stamina > _playerData.Stamina)
            {
                _stamina = _playerData.Stamina;
            }
            yield return null;
        }
        _staminaRegenCoroutine = null;
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
