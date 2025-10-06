using TMPro;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _damageText;
    private Animator _animator;

    public void SetDamage(string damage)
    {
        _damageText.text = damage;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
