using UnityEngine;

public class ShipWheel : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject arrow;
    private IInteractable Interactable;


    public void SetActive()
    {
        Interactable.SetActive();
        arrow.SetActive(true);
    }

    public void SetInactive()
    {
        Interactable.SetInactive();
        arrow.SetActive(false);
    }

    private void Awake()
    {
        Interactable = GetComponentInChildren<Interactable>();
        Interactable.Register(OpenMap);
        SetInactive();
    }

    private void OpenMap(GameObject player)
    {
        if (map == null)
        {
            player.GetComponent<CharacterUIManager>().ToggleMapUI();
            return;
        }

        map.SetActive(true);
    }
}
