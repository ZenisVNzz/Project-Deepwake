using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTreePanel : MonoBehaviour
{
    [Header("Skill Nodes")]
    [SerializeField] private Transform skillNodeParent;
    [SerializeField] private GameObject skillNodePrefab;

    [Header("Skill Select")]
    [SerializeField] private Transform skillListParent;
    [SerializeField] private GameObject skillButtonPrefab;

    [Header("Skill Description")]
    [SerializeField] private Text skillNameText;
    [SerializeField] private Text skillDescriptionText;
    [SerializeField] private Text skillCostText;

    private List<SkillData> allSkills = new();

    private void Start()
    {
        LoadSkills();
    }

    public void LoadSkills()
    {
        foreach (Transform child in skillNodeParent)
            Destroy(child.gameObject);
        foreach (Transform child in skillListParent)
            Destroy(child.gameObject);

        allSkills.AddRange(Resources.LoadAll<SkillData>("Skills"));

        foreach (var skill in allSkills)
        {
            var button = Instantiate(skillButtonPrefab, skillListParent);
            button.GetComponentInChildren<Text>().text = skill.skillName;
            button.GetComponent<Button>().onClick.AddListener(() => ShowSkillDetail(skill));

            var node = Instantiate(skillNodePrefab, skillNodeParent);
            node.GetComponent<UISkillNode>().Init(skill);
        }
    }

    private void ShowSkillDetail(SkillData skill)
    {
        skillNameText.text = skill.skillName;
        skillDescriptionText.text = skill.description;
        skillCostText.text = $"Cost: {skill.pointCost}";
    }

}