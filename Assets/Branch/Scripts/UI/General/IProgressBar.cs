using UnityEngine;

public interface IProgressBar
{
    void Initialize(float maxValue);
    void SetValue(float currentValue);
    void UpdateBar();
}
