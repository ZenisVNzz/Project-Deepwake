using UnityEngine;

public class ObjectShadowCast : MonoBehaviour
{
    private GameObject _shadowObject;

    private void Awake()
    {
        CastShadow();
    }

    private void CastShadow()
    {
        if (_shadowObject != null) return;

        SpriteRenderer originalRenderer = GetComponent<SpriteRenderer>();

        if (originalRenderer == null) return;
        _shadowObject = new GameObject($"{name}_Shadow");
        _shadowObject.transform.SetParent(transform);

        _shadowObject.transform.localScale = new Vector3(1f, 1.5f, 1f);
        _shadowObject.transform.localPosition = new Vector3(0f, 0.04f, 0f);
        _shadowObject.transform.localRotation = Quaternion.Euler(45f, 0f, -100f);

        SpriteRenderer shadowRenderer = _shadowObject.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = originalRenderer.sprite;
        shadowRenderer.color = new Color(0f, 0f, 0f, 0.6f);
        shadowRenderer.sortingLayerID = originalRenderer.sortingLayerID;
        shadowRenderer.sortingOrder = originalRenderer.sortingOrder - 1;

        GameTimeController gameTimeController = FindFirstObjectByType<GameTimeController>();
        if (gameTimeController != null && !gameTimeController.ShadowCasters.Contains(this))
        {
            gameTimeController.ShadowCasters.Add(this);
        }
    }

    public void transparency(float value)
    {
        _shadowObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, value);
    }

    public void UpdateShadowTransparency(int hour, int minute = 0)
    {
        float timeOfDay = hour + minute / 60f;
        float alpha;

        if (timeOfDay >= 6f && timeOfDay <= 18f)
        {
            float t = (timeOfDay - 6f) / 12f;
            alpha = Mathf.Lerp(0.3f, 0.6f, Mathf.Cos((t - 0.5f) * Mathf.PI) * 0.5f + 0.5f);
        }
        else if (timeOfDay >= 5f && timeOfDay < 6f)
        {
            float t = (timeOfDay - 5f) / 1f;
            alpha = Mathf.Lerp(0.1f, 0.3f, t);
        }
        else if (timeOfDay > 18f && timeOfDay <= 19f)
        {
            float t = (timeOfDay - 18f) / 1f;
            alpha = Mathf.Lerp(0.3f, 0.1f, t);
        }
        else
        {
            alpha = 0.1f;
        }

        transparency(alpha);
    }

    public void RotateByTime(int hour, int minute)
    {
        float t = (hour + minute / 60f) / 24f; 
        float angle = t * 360f; 

        _shadowObject.transform.localRotation = Quaternion.Euler(45f, 0f, angle);
    }
}
