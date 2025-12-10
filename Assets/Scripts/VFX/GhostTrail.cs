using System.Collections;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private float spawnInterval = 0.1f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Init()
    {
        while (true)
        {
            GameObject gameObject = Instantiate(prefab, transform.position, transform.rotation);
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            Animator animator = gameObject.GetComponent<Animator>();
            sr.sprite = spriteRenderer.sprite;
            sr.flipX = spriteRenderer.flipX;
            Destroy(gameObject, 0.2f);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StartTrail(float time)
    {
        StartCoroutine(Init());
        Invoke("StopTrail", time);
    }

    public void StopTrail()
    {
        StopAllCoroutines();
    }
}
