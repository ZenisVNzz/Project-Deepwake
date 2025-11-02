using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
