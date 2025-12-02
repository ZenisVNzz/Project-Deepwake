using UnityEngine;

public class SFXTrigger : MonoBehaviour
{
    public SFXData sfx;

    public void Play()
    {
        Debug.Log($"Playing SFX: {sfx.name} at position {transform.position}");
        SFXManager.Instance.Play(sfx, transform.position);
    }

}