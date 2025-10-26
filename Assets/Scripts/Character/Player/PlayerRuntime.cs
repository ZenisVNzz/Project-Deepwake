using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRuntime : CharacterRuntime, IPlayerRuntime
{   
    private Coroutine _staminaRegenCoroutine;

    public override void Init(CharacterData playerData, Rigidbody2D rigidbody2D, IState PlayerState)
    {
        base.Init(playerData, rigidbody2D, PlayerState);
    }

    public bool UseStamina(float amount)
    {
        float adjustedAmount = amount * _staminaConsumptionMultiplier;
        if (stamina >= adjustedAmount)
        {
            stamina -= adjustedAmount;
            _onStatusChanged?.Invoke();

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
        while (stamina < totalStamina)
        {
            stamina += _staminaRegenRate * Time.deltaTime;           
            if (stamina > totalStamina)
            {
                stamina = totalStamina;
            }
            _onStatusChanged?.Invoke();
            yield return null;
        }
        _staminaRegenCoroutine = null;
    }
}
