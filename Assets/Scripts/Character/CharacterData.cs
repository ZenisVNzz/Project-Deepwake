using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Data/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public int characterID;
    public GameObject prefab;

    public int level;
    public float HP;
    public float Stamina;
    public float AttackPower;
    public float Defense;
    public float MoveSpeed;
    public float CriticalChance;
    public float CriticalDamageMultiplier;

    public float HPRegenRate;
    public float StaminaRegenRate;
    public float StaminaConsumptionMultiplier;

    public Skill skill;
    public Passive Passive1;
    public Passive Passive2;
}
