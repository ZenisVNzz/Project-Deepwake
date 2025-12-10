using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer masterMixer;

    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", ConvertToDB(volume));
    }

    public void SetBGMVolume(float volume)
    {
        masterMixer.SetFloat("BGMVolume", ConvertToDB(volume));
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFXVolume", ConvertToDB(volume));
    }

    private float ConvertToDB(float volume)
    {
        return volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
    }
}
