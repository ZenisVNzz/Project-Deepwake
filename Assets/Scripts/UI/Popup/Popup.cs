using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private string InstanceID;
    [SerializeField] private LocalizationText contentText;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Setup(string InstanceID ,LocalizedString content, Action button1Action, Action button2Action)
    {
        this.InstanceID = InstanceID;

        contentText.ChangeText(content);

        if (button1 != null)
        {
            button1.onClick.AddListener(() =>
            {
                button1Action?.Invoke();
                StartCoroutine(ClosePopup(animator.GetCurrentAnimatorStateInfo(0).length));
            });
        }   

        if (button2 != null)
        {
            button2.onClick.AddListener(() =>
            {
                button2Action?.Invoke();
                StartCoroutine(ClosePopup(animator.GetCurrentAnimatorStateInfo(0).length));
            });
        }
    }

    public void Setup(string InstanceID, LocalizedString content) => Setup(InstanceID, content, null, null);

    private IEnumerator ClosePopup(float destroyTime)
    {
        animator.Play("Popup_Close");
        yield return new WaitForSeconds(destroyTime);
        UIManager.Instance.GetPopupService().Destroy(InstanceID, 0f);
    }
}
