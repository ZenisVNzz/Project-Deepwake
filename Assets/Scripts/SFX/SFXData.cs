using UnityEngine;

[CreateAssetMenu(fileName = "SFXData", menuName = "GameData/SFX Data")]
public class SFXData : ScriptableObject
{
    public string sfxID;
    public AudioClip audioClip;
    public float volume = 1f;
    public bool loop = false;
}