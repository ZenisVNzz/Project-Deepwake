using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Setup(string content, Action button1Action, Action button2Action)
    {
        contentText.text = content;

        if (button1Action != null)
        {
            button1.onClick.AddListener(() =>
            {
                button1Action?.Invoke();
                animator.Play("Popup_Close");
                Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            });
        }   

        if (button2Action != null)
        {
            button2.onClick.AddListener(() =>
            {
                button2Action?.Invoke();
                animator.Play("Popup_Close");
                Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            });
        }
    }

    public void Setup(string content) => Setup(content, null, null);
}
