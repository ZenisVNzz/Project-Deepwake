using UnityEngine;

[CreateAssetMenu(fileName = "AmbientLayer", menuName = "Audio/Ambient Layer")]
public class AmbientLayerData : ScriptableObject
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 2f)] public float pitch = 1f;
    public bool loop = true;
    public float duration = 1f;

    [Header("Fade Settings")]
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    [Header("Segment Settings")]
    public bool useSegment = false;
    public float startTime = 0f;
    public float endTime = 0f;

    [Header("Random Play Delay")]
    public float minDelay = 2f;
    public float maxDelay = 5f;
}