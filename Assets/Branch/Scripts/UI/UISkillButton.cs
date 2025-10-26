using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UISkillButton : MonoBehaviour
{
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Button upgradeButton;

    private SkillData skill;

    public void Bind(SkillData s)
    {
        skill = s;
        Refresh();
        upgradeButton.onClick.AddListener(LevelUp);
    }

    private void Refresh()
    {
        skillName.text = skill.skillName;
        levelText.text = $"{skill.level}/{skill.maxLevel}";
        upgradeButton.interactable = skill.level < skill.maxLevel;
    }

    private void LevelUp()
    {
        if (skill.level < skill.maxLevel)
        {
            skill.level++;
            Refresh();
        }
    }
}