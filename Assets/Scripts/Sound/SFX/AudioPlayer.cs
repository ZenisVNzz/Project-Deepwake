using UnityEngine;

public class AudioPlayer : MonoBehaviour, IPlaySFX
{
    [SerializeField] private AudioPool audioPool;

    public void PlaySFX(SFXData data, Vector3 position)
    {
        var audioObj = audioPool.Get();
        audioObj.Play(data, position);
    }
}