using UnityEngine;
using System.Collections;

public class AudioObject : MonoBehaviour
{
    private AudioSource source;
    private AudioPool pool;

    private Coroutine playingRoutine;

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

        if (playingRoutine != null)
            StopCoroutine(playingRoutine);

        if (!data.useSegment)
        {
            source.time = 0f;
            source.Play();

            if (!data.loop)
                playingRoutine = StartCoroutine(ReturnAfterDelay(data.duration));

            return;
        }

        playingRoutine = StartCoroutine(PlaySegment(data));

    }

    private IEnumerator PlaySegment(SFXData data)
    {
        float clipLen = source.clip.length;

        float start = Mathf.Clamp(data.startTime, 0f, clipLen);
        float end = Mathf.Clamp(data.endTime, start, clipLen);

        source.time = start;
        source.Play();

        while (source.time < end)
            yield return null;

        source.Stop();
        pool.ReturnToPool(this);
    }

    private IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnToPool(this);
    }

    public void Stop()
    {
        if (playingRoutine != null)
            StopCoroutine(playingRoutine);

        source.Stop();
        pool.ReturnToPool(this);
    }
}