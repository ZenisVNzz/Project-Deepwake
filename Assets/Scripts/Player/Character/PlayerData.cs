using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public float HP;
    public float Stamina;
    public float AttackPower;
    public float Defense;
    public float MoveSpeed;
    public float CriticalChance;
    public float CriticalDamageMultiplier;

    public Skill skill;
    public Passive Passive1;
    public Passive Passive2;
}
