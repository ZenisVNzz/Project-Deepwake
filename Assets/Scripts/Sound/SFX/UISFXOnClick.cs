using UnityEngine;
using UnityEngine.UI;

public class UISFX_OnClick : MonoBehaviour
{
    public SFXData sfx;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (sfx != null)
            SFXManager.Instance.Play(sfx, Vector3.zero);
    }
}