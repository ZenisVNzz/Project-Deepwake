using UnityEngine;

public class EnemyRuntime : CharacterRuntime
{
    public override void Init()
    {
        characterData = GetComponent<CharacterInstaller>()._characterData;
        hp = totalHealth;
        _hpRegenRate = characterData.HPRegenRate;

        rb = GetComponent<Rigidbody2D>();
        this.characterState = GetComponent<EnemyController>().enemyState;
    }
}
