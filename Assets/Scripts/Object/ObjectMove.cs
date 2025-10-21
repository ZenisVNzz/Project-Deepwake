using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
