using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private LocalizationText contentText;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Setup(LocalizedString content, Action button1Action, Action button2Action)
    {
        contentText.ChangeText(content);

        if (button1 != null)
        {
            button1.onClick.AddListener(() =>
            {
                button1Action?.Invoke();
                animator.Play("Popup_Close");
                Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            });
        }   

        if (button2 != null)
        {
            button2.onClick.AddListener(() =>
            {
                button2Action?.Invoke();
                animator.Play("Popup_Close");
                Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            });
        }
    }

    public void Setup(LocalizedString content) => Setup(content, null, null);
}
