using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    public List<GameObject> fabeObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FadeObject();
        }

    }

    public void FadeObject()
    {
        foreach (var obj in fabeObject)
        {
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 0.4f;
                spriteRenderer.color = color;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnfadeObject();
        }
    }

    public void UnfadeObject()
    {
        foreach (var obj in fabeObject)
        {
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                var color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }
        }
    }
}
