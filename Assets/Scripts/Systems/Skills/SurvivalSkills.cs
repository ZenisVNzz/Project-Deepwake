//using UnityEngine;

//public class SurvivalSkills : MonoBehaviour
//{
//    [CreateAssetMenu(menuName = "Skills/Endurance")]
//    public class EnduranceSkill : Skill
//    {
//        public float healthBonus = 20f;

//        public override void ApplySkill(GameObject player)
//        {
//            PlayerStats stats = player.GetComponent<PlayerStats>();
//            if (stats != null) stats.maxHealth += healthBonus * level;
//        }
//    }

//    [CreateAssetMenu(menuName = "Skills/BloodRecovery")]
//    public class BloodRecoverySkill : Skill
//    {
//        public float regenBonus = 2f;

//        public override void ApplySkill(GameObject player)
//        {
//            PlayerStats stats = player.GetComponent<PlayerStats>();
//            if (stats != null) stats.healthRegen += regenBonus * level;
//        }
//    }

//    [CreateAssetMenu(menuName = "Skills/ResourceManagement")]
//    public class ResourceManagementSkill : Skill
//    {
//        public float usageEfficiency = 0.8f;

//        public override void ApplySkill(GameObject player)
//        {
//            PlayerInventory inv = player.GetComponent<PlayerInventory>();
//            if (inv != null) inv.resourceUsageMultiplier *= usageEfficiency;
//        }
//    }

//    [CreateAssetMenu(menuName = "Skills/ItemCrafting")]
//    public class ItemCraftingSkill : Skill
//    {
//        public override void ApplySkill(GameObject player)
//        {
//            PlayerCrafting craft = player.GetComponent<PlayerCrafting>();
//            if (craft != null) craft.unlockNewItems = true;
//        }
//    }

//    [CreateAssetMenu(menuName = "Skills/WaterPurification")]
//    public class WaterPurificationSkill : Skill
//    {
//        public float waterBonus = 1.5f;

//        public override void ApplySkill(GameObject player)
//        {
//            PlayerSurvival survival = player.GetComponent<PlayerSurvival>();
//            if (survival != null) survival.waterEfficiency *= waterBonus;
//        }
//    }

//}
