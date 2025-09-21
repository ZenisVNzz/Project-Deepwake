using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "DataSystem/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Graphics")]
    public int Quality;
    public int Fps;
    public bool CameraShake;

    [Header("Audio")]
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;

    [Header("Gameplay")]
    public bool ShowDamageNumber;
}
