using UnityEngine;

public class PlayerCheat : MonoBehaviour
{
    [ContextMenu("Player Add 99 Attributes point")]
    private void TestFunction()
    {
        CharacterUIManager characterUIManager = GetComponent<CharacterUIManager>();
        characterUIManager.GrantAttributePoints(99);
    }
}
