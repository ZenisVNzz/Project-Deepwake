using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    // Expose speed so other components (e.g., ShipController) can control movement/acceleration
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
