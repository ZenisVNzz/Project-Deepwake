using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRuntime : CharacterRuntime, IPlayerRuntime
{
    [SerializeField] private float _stamina;
    public float Stamina => _stamina;

    private float _staminaRegenRate;
    private float _staminaConsumptionMultiplier;
    private Coroutine _staminaRegenCoroutine;

    private CharacterData _playerData;
    public CharacterData PlayerData => _playerData;

    public override void Init(CharacterData playerData, Rigidbody2D rigidbody2D)
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
}
