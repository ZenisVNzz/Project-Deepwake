using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance { get; private set; }
    public event Action<SkillData> OnSkillUnlocked;

    private HashSet<SkillData> unlockedSkills = new();
    public int skillPoints = 5;

    private void Awake() => Instance = this;

    public bool CanUnlock(SkillData skill)
    {
        if (unlockedSkills.Contains(skill)) return false;
        if (skill.prerequisite && !unlockedSkills.Contains(skill.prerequisite)) return false;
        return skillPoints >= skill.pointCost;
    }

    public void Unlock(SkillData skill)
    {
        if (!CanUnlock(skill)) return;

        skillPoints -= skill.pointCost;
        unlockedSkills.Add(skill);
        OnSkillUnlocked?.Invoke(skill);

        Debug.Log($"Unlocked skill: {skill.skillName}");
    }

    public bool IsUnlocked(SkillData skill) => unlockedSkills.Contains(skill);
}