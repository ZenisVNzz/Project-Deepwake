using DG.Tweening;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float floatAmplitude = 0.5f;
    [SerializeField] private float floatDuration = 2f;   

    private void Start()
    {
        Vector3 startPos = transform.localPosition;

        transform.DOLocalMoveY(startPos.y + floatAmplitude, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}