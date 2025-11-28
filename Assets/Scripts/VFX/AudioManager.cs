using UnityEngine;

public static class AudioManager
{
    public static void PlayOneShot(AudioClip clip, Vector3 pos)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, pos);
    }
}