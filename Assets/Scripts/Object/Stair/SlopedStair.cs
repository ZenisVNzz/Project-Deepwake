using UnityEngine;

public class SlopedStair : MonoBehaviour
{
    public bool reverseSlope = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var controller = collision.GetComponentInParent<PlayerController>();
            controller.MoveOnSlope(true);

            if (reverseSlope)
            {
                controller.reverseSlope = true;
            }
            else
            {
                controller.reverseSlope = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var controller = collision.GetComponentInParent<PlayerController>();
            controller.MoveOnSlope(false);
        }
    }
}
