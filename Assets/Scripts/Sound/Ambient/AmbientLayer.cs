using UnityEngine;

public class AmbientLayer
{
    public AmbientLayerData data;
    public AudioSource source;
    public Coroutine routine;

    public AmbientLayer(AmbientLayerData data, AudioSource source)
    {
        this.data = data;
        this.source = source;
    }
}