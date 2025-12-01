using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource source;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void Play(MusicData music)
    {
        if (music == null || music.clip == null)
            return;

        source.clip = music.clip;
        source.volume = music.volume;
        source.loop = music.loop;

        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}
