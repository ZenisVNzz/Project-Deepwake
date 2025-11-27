using UnityEngine;
using UnityEngine.UI;

public class CloseUIButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private GameObject ObjToClose;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            ObjToClose.SetActive(false);
        });
    }
}
