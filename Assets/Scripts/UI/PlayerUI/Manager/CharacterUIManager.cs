using System.Collections.Generic;
using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private UIStats statsPanel;
    [SerializeField] private UIAttributesPanel attributesPanel;
    [SerializeField] private UIEquipmentPanel equipmentPanel;
    [SerializeField] private UISkillTreePanel skillPanel;

    [Header("Data")]
    [SerializeField] private CharacterRuntime stats;
    [SerializeField] private CharacterAttributes attributes;
    [SerializeField] private Equipment equipment;
    [SerializeField] private List<SkillData> skills;

    private void Start()
    { 
        attributesPanel.Bind(attributes);
        statsPanel.UpdateStats(stats);

        skillPanel.LoadSkills();
    }

    private void Update()
    {
        statsPanel.UpdateStats(stats);
    }
}
