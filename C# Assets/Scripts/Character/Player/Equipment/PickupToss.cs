using System.Collections;
using UnityEngine;

public class ItemToss2D : MonoBehaviour
{
    [Header("Toss Settings")]
    [SerializeField] private float launchSpeed = 3f;        
    [SerializeField] private float airDamping = 3f;        
    [SerializeField] private float jumpHeight = 1f;         
    [SerializeField] private float gravity = 5f;            
    [SerializeField] private float stopThreshold = 0.1f;    

    [Header("Shadow Reference")]
    [SerializeField] private Transform shadowTransform;     

    private Vector2 moveDir;
    private float verticalVelocity;
    private float height;

    public void Launch(Vector2 direction)
    {
        moveDir = direction.normalized;
        verticalVelocity = Mathf.Sqrt(2f * gravity * jumpHeight); 
        StartCoroutine(TossRoutine());
    }

    public void LaunchRandom(float minAngle = 0f, float maxAngle = 360f)
    {
        float angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
        Launch(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
    }

    private IEnumerator TossRoutine()
    {
        float speed = launchSpeed;

        while (true)
        {
            height += verticalVelocity * Time.deltaTime;
            verticalVelocity -= gravity * Time.deltaTime;

            if (height < 0f)
            {
                height = 0f;
                break;
            }

            transform.position += (Vector3)(moveDir * speed * Time.deltaTime);

            speed = Mathf.MoveTowards(speed, 0f, airDamping * Time.deltaTime);

            if (shadowTransform != null)
            {
                float scale = 1f - Mathf.Clamp01(height / jumpHeight) * 0.4f;
                shadowTransform.localScale = new Vector3(scale, scale, 1f);
            }

            yield return null;
        }

        height = 0f;
        if (shadowTransform != null)
            shadowTransform.localScale = Vector3.one;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (shadowTransform == null)
        {
            Transform found = transform.Find("Shadow");
            if (found) shadowTransform = found;
        }
    }
#endif
}
