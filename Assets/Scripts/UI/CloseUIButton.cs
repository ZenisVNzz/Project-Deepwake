using UnityEngine;
using UnityEngine.UI;

public class CloseUIButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private GameObject ObjToClose;
    private SFXData ClickSFX = ResourceManager.Instance.GetAsset<SFXData>("UIButtonSFX");

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            ObjToClose.SetActive(false);
        });
        SFXManager.Instance.Play(ClickSFX, transform.position);
    }
}
