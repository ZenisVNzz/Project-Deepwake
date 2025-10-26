using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour
{
    [SerializeField] private float detectRange = 2f;
    [SerializeField] private LayerMask interactableMask;

    private IInteractable currentTarget;
    private Character _owner;

    private void Start()
    {
        _owner = GetComponent<Character>();
    }

    private void Update()
    {
        DetectInteractable();
        HandleInteractionInput();
    }

    private void DetectInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRange, interactableMask);

        IInteractable nearest = null;
        float nearestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            var interactable = hit.GetComponent<IInteractable>();
            if (interactable == null) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = interactable;
            }
        }

        if (nearest != currentTarget)
        {
            currentTarget = nearest;
            UpdateInteractionUI(nearest);
        }
    }

    private void HandleInteractionInput()
    {
        if (currentTarget == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentTarget.Interact(_owner);
        }
    }

    private void UpdateInteractionUI(IInteractable target)
    {
        var uiManager = FindObjectOfType<UIManager>();
        var interactUI = uiManager.GetUI<UIInteractionPrompt>("InteractionPrompt");

        if (interactUI == null) return;

        if (target != null)
            interactUI.ShowPrompt(target.InteractionPrompt);
        else
            interactUI.HidePrompt();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
