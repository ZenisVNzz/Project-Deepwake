using System.Collections.Generic;
using UnityEngine;

public class SFXPlay : MonoBehaviour
{
    public void PlaySFX(string sfxKeys)
    {
        SFXData sfxData = ResourceManager.Instance.GetAsset<SFXData>(sfxKeys);
        SFXManager.Instance.Play(sfxData, transform.position);
    }
}
