using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Music Data")]
public class MusicData : ScriptableObject
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 2f)] public float pitch = 1f;
    public bool loop;
    public float duration = 1f;

    [Header("Segment Settings")]
    public bool useSegment = false;
    public float startTime = 0f;
    public float endTime = 0f;
}
