using UnityEngine;

[CreateAssetMenu(fileName = "VFXData", menuName = "Game/VFX Data")]
public class VFXData : ScriptableObject
{
    public string vfxID;

    public GameObject prefab;

    public AudioClip sfx;

    public float lifeTime = 0.5f;

    public int poolSize = 5;
}