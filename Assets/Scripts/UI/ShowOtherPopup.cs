using UnityEngine;
using UnityEngine.UI;

public class ShowOtherPopup : MonoBehaviour
{
    public Button showPopupButton;
    public GameObject targetPopup;
    public GameObject currentPopup;
    public bool closeCurrentPopup = true;

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
    }
}
