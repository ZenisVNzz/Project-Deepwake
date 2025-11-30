using UnityEngine;
using System.Collections;

public class AudioObject : MonoBehaviour
{
    private AudioSource source;
    private AudioPool pool;

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    public void Init(AudioPool assignedPool)
    {
        pool = assignedPool;
    }

    public void Play(SFXData data, Vector3 position)
    {
        transform.position = position;

        source.clip = data.clip;
        source.volume = data.volume;
        source.pitch = data.pitch;
        source.loop = data.loop;

        source.Play();

        if (!data.loop)
            StartCoroutine(ReturnAfterDelay(data.duration));
    }

    private IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnToPool(this);
    }

    public void Stop()
    {
        source.Stop();
        pool.ReturnToPool(this);
    }
}