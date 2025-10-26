using UnityEngine;
using UnityEngine.UI;

public class UISkillNode : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button unlockButton;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private SkillData skillData;

    private void Start()
    {
        icon.sprite = skillData.icon;
        UpdateVisual();
        unlockButton.onClick.AddListener(OnUnlockClicked);
        SkillTreeManager.Instance.OnSkillUnlocked += OnSkillUnlocked;
    }

    private void OnDestroy()
    {
        SkillTreeManager.Instance.OnSkillUnlocked -= OnSkillUnlocked;
    }

    private void OnUnlockClicked()
    {
        SkillTreeManager.Instance.Unlock(skillData);
    }

    private void OnSkillUnlocked(SkillData unlocked)
    {
        if (unlocked == skillData || skillData.prerequisite == unlocked)
            UpdateVisual();
    }

    private void UpdateVisual()
    {
        bool unlocked = SkillTreeManager.Instance.IsUnlocked(skillData);
        bool canUnlock = SkillTreeManager.Instance.CanUnlock(skillData);

        lockedOverlay.SetActive(!unlocked);
        unlockButton.interactable = canUnlock;
    }

    public void Init(SkillData data)
    {
        skillData = data;
        icon.sprite = data.icon;
        UpdateVisual();
    }

}