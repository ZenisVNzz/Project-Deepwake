using UnityEngine;

public class EnemyLevelModifier
{
    private float HPMultipler = 1.18f;
    private float StaminaMultiplier = 1.11f;
    private float AttackMultiplier = 1.15f;
    private float DefenseMultiplier = 1.12f;

    public CharacterData ModifyStats(CharacterData charData, int level)
    {
        CharacterData charDataInstance = GameObject.Instantiate(charData);
        charDataInstance.level = level;
        charDataInstance.HP *= Mathf.Pow(HPMultipler, level - 1);
        charDataInstance.Stamina *= Mathf.Pow(StaminaMultiplier, level - 1);
        charDataInstance.AttackPower *= Mathf.Pow(AttackMultiplier, level - 1);
        charDataInstance.Defense *= Mathf.Pow(DefenseMultiplier, level - 1);
        return charDataInstance;
    }
}
