using UnityEngine;

public class SlopedStair : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var controller = collision.GetComponentInParent<IPlayerController>();
            controller.MoveOnSlope(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var controller = collision.GetComponentInParent<IPlayerController>();
            controller.MoveOnSlope(false);
        }
    }
}
