using UnityEngine;
using UnityEngine.UI;

public class ShowOtherPopup : MonoBehaviour
{
    public Button showPopupButton;
    public GameObject targetPopup;
    public GameObject currentPopup;
    public bool closeCurrentPopup = true;

    private SFXData ClickSFX => ResourceManager.Instance.GetAsset<SFXData>("UIButtonSFX");

    void Start()
    {
        showPopupButton.onClick.AddListener(ShowPopup);
    }

    void ShowPopup()
    {
        targetPopup.SetActive(true);
        if (closeCurrentPopup && currentPopup != null)
        {
            currentPopup.SetActive(false);
        }
        SFXManager.Instance.Play(ClickSFX, transform.position);
    }
}
