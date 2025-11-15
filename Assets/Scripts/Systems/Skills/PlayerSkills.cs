//using JetBrains.Annotations;
//using NUnit.Framework;
//using UnityEngine;

//public class PlayerSkills : MonoBehaviour
//{
//    public List<Skill> unlockedSkills = new List<Skill>();

//    public void LearnSkill(Skill newSkill)
//    {
//        if (!unlockedSkills.Contains(newSkill))
//        {
//            unlockedSkills.Add(newSkill);
//            Debug.Log($"Learned new skill: {newSkill.skillName}");
//        }
//        else
//        {
//            Debug.LogWarning($"Skill {newSkill.skillName} is already learned.");
//        }
//    }

//    public void UpdrageSkill(Skill skill)
//    {
//        if(unlockedSkills.Contains(skill) && skill.level < skill.maxLevel)
//        {
//            skill.level++;
//            skill.ApplySkill(this.gameObject);
//            Debug.Log($"Upgraded skill: {skill.skillName} to level {skill.level}");
//        }
//        else
//        {
//            Debug.LogWarning($"Cannot upgrade skill: {skill.skillName}. It may not be learned or is already at max level.");
//        }
//    }
//}
