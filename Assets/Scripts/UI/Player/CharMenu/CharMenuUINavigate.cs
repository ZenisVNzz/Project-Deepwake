using UnityEngine;
using UnityEngine.UI;

public class CharMenuUINavigate : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPopup;
    [SerializeField] private Button inventoryUIButton;
    [SerializeField] private GameObject statusPopup;
    [SerializeField] private Button statusUIButton;
    [SerializeField] private GameObject skillTreePopup;
    [SerializeField] private Button skillTreeButton;
    private GameObject currentPopup;
    private SFXData ClickSFX = ResourceManager.Instance.GetAsset<SFXData>("UIButtonSFX");

    private void Start()
    {
        OpenPopup(inventoryPopup);
        inventoryUIButton.onClick.AddListener(() => OpenPopup(inventoryPopup));
        statusUIButton.onClick.AddListener(() => OpenPopup(statusPopup));
        skillTreeButton.onClick.AddListener(() => OpenPopup(skillTreePopup));
    }

    private void OpenPopup(GameObject popupToOpen)
    {
        Debug.Log($"Opening popup: {popupToOpen.name}");
        if (currentPopup != null)
        {
            currentPopup.SetActive(false);
        }
        popupToOpen.SetActive(true);
        currentPopup = popupToOpen;
        SFXManager.Instance.Play(ClickSFX, transform.position);
    }
}
