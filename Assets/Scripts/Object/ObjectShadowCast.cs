using UnityEngine;

[ExecuteInEditMode]
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
    }
}
