using UnityEngine;
using UnityEngine.UI;

public class FlyAttackSFX : MonoBehaviour
{
    public SFXData SFX = ResourceManager.Instance.GetAsset<SFXData>("FlyAttackSFX");

    void Start()
    {
        SFXManager.Instance.Play(SFX, transform.position);
    }


}