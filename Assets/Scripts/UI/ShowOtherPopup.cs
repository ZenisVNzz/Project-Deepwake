using UnityEngine;
using UnityEngine.UI;

public class ShowOtherPopup : MonoBehaviour
{
    public Button showPopupButton;
    public GameObject targetPopup;

    void Start()
    {
        showPopupButton.onClick.AddListener(ShowPopup);
    }

    void ShowPopup()
    {
        targetPopup.SetActive(true);
    }
}
