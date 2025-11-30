using UnityEngine;

[CreateAssetMenu(fileName = "SFXData", menuName = "Audio/SFX Data")]
public class SFXData : ScriptableObject
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 2f)] public float pitch = 1f;
    public bool loop;
    public float duration = 1f;
}