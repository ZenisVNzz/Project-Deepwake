using UnityEngine;

[CreateAssetMenu(menuName = "Game/VFX Data")]
public class VFXData : ScriptableObject
{
    public string vfxID;
    public BaseVFX prefab;
    public AudioClip sfx;
    public float lifeTime = 2f;
    public int poolSize = 5;
}