using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientManager : MonoBehaviour
{
    public static AmbientManager Instance;

    private class AmbientLayer
    {
        public AudioSource source;
        public Coroutine fadeCoroutine;
    }

    private Dictionary<string, AmbientLayer> layers = new Dictionary<string, AmbientLayer>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayLayer(string layerName, AmbientLayerData data)
    {
        if (data == null || data.clip == null)
            return;

        if (!layers.ContainsKey(layerName))
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.loop = data.loop;
            source.clip = data.clip;
            source.playOnAwake = false;

            layers[layerName] = new AmbientLayer()
            {
                source = source,
                fadeCoroutine = null
            };
        }

        AmbientLayer layer = layers[layerName];

        if (layer.fadeCoroutine != null)
            StopCoroutine(layer.fadeCoroutine);

        layer.source.clip = data.clip;
        layer.source.loop = data.loop;

        layer.fadeCoroutine = StartCoroutine(FadeIn(layer.source, data.volume, data.fadeInTime));
    }

    public void StopLayer(string layerName, AmbientLayerData data)
    {
        if (!layers.ContainsKey(layerName))
            return;

        AmbientLayer layer = layers[layerName];

        if (layer.fadeCoroutine != null)
            StopCoroutine(layer.fadeCoroutine);

        layer.fadeCoroutine = StartCoroutine(FadeOut(layer.source, data.fadeOutTime));
    }

    private IEnumerator FadeIn(AudioSource src, float targetVolume, float duration)
    {
        src.volume = 0f;
        src.Play();

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        src.volume = targetVolume;
    }

    private IEnumerator FadeOut(AudioSource src, float duration)
    {
        float startVolume = src.volume;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        src.Stop();
        src.volume = 0f;
    }


}