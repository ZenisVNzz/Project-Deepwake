using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    private IPlaySFX player;

    [Header("Mixer")]
    public AudioMixerGroup sfxMixerGroup;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        player = GetComponent<IPlaySFX>();
        if (player == null)
            player = gameObject.AddComponent<AudioPlayer>();
    }

    public void Play(SFXData data, Vector3 pos)
    {
        player.PlaySFX(data, pos);
    }
}