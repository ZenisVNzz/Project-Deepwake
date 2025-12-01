using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private IPlaySFX player;

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