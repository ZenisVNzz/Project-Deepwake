using UnityEngine;
using UnityEngine.InputSystem;

public class CannonBulletRuntime : MonoBehaviour
{
    private Rigidbody2D rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}