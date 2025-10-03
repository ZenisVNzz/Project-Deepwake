using UnityEngine;

public class ObjectShadowCast : MonoBehaviour
{
    private GameObject _shadowObject;
    [SerializeField] private bool updateShadowSprite = false;

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

    public void UpdateShadowTransparency(int hour)
    {
        float alpha;

        if (hour >= 6 && hour <= 18)
        {
            float t = (hour - 6) / 12f;
            alpha = Mathf.Lerp(0.3f, 0.5f, 1f - Mathf.Abs(t - 0.5f) * 2f);
        }
        else if (hour > 18 && hour <= 24)
        {
            float t = (hour - 18) / 6f;
            alpha = Mathf.Lerp(0.1f, 0.05f, t);
        }
        else if (hour >= 0 && hour < 6)
        {
            float t = hour / 6f;
            alpha = Mathf.Lerp(0.05f, 0.28f, t);
        }
        else
        {
            alpha = 0.05f;
        }

        transparency(alpha);
    }

    public void RotateByTime(int hour, int minute)
    {
        float t = (hour + minute / 60f) / 24f; 
        float angle = t * 360f; 

        _shadowObject.transform.localRotation = Quaternion.Euler(45f, 0f, angle);
    }

    private void LateUpdate()
    {
        if (_shadowObject == null) return;

        if (updateShadowSprite)
        {
            SpriteRenderer originalRenderer = GetComponent<SpriteRenderer>();
            SpriteRenderer shadowRenderer = _shadowObject.GetComponent<SpriteRenderer>();

            if (originalRenderer != null && shadowRenderer != null && shadowRenderer.sprite != originalRenderer.sprite)
            {
                shadowRenderer.sprite = originalRenderer.sprite;
            }
        }
    }
}
