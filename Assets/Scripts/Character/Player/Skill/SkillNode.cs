using UnityEngine;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    [SerializeField] private SkillData skillData;

    private void Start()
    {
        icon.sprite = skillData.icon;
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        SkillTreeManager.Instance.Unlock(skillData);
    }
}