using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill")]
public class SkillData : ScriptableObject
{
    public string skillId;
    public string skillName;
    public string description;
    public Sprite icon;
    public int level;
    public int maxLevel;
    public float cooldown;
    public SkillType type;
    public SkillData prerequisite;
    public int pointCost;
}

public enum SkillType
{
    Attack,
    Defense,
    Heal,
    Support,
    Special
}