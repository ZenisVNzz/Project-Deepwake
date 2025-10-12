using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private Animator _animator;

    public void SetText(string damage)
    {
        _text.text = damage;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
