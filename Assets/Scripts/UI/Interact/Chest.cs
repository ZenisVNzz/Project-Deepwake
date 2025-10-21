using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Click to Open";
    public string InteractionPrompt => prompt;

    private bool isOpened = false;

    public void Interact(Character interactor)
    {
        if (isOpened) return;
        isOpened = true;


        Debug.Log($"{interactor.name} open Chest!");
    }
}
