using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class FloatingJoyStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Settings")]
    [SerializeField] private float m_MovementRange = 100f;
    [SerializeField] private Canvas m_Canvas;

    [Header("References")]
    [SerializeField] private RectTransform m_Background;
    [SerializeField] private RectTransform m_Handle;

    [InputControl(layout = "Vector2")]
    [SerializeField] private string m_ControlPath;

    private Vector2 m_StartPos;
    private Vector2 m_PointerDownPos;
    private Vector2 m_DragPos;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    private void Awake()
    {
        if (m_Canvas == null)
            m_Canvas = GetComponentInParent<Canvas>();

        if (m_Background == null || m_Handle == null)
            Debug.LogWarning("[FloatingJoyStick] Background or Handle is not assigned.");

        m_Background.gameObject.SetActive(false);
    }

    private Camera GetCanvasCamera()
    {
        if (m_Canvas == null) return null;
        return m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_Canvas.worldCamera;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        if (eventData.position.x > Screen.width / 2f)
            return;

        if (m_Canvas == null)
        {
            Debug.LogWarning("[FloatingJoyStick] Canvas reference missing.");
            return;
        }

        var canvasRect = m_Canvas.transform as RectTransform;
        var cam = GetCanvasCamera();

        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, cam, out localPoint))
            localPoint = Vector2.zero;

        if (m_Canvas.scaleFactor != 0f)
            localPoint /= m_Canvas.scaleFactor;

        m_PointerDownPos = localPoint;

        if (m_Background != null)
            m_Background.anchoredPosition = m_PointerDownPos;

        if (m_Handle != null)
            m_Handle.anchoredPosition = Vector2.zero;

        if (m_Background != null)
            m_Background.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        if (eventData.position.x > Screen.width / 2f)
            return;

        if (m_Canvas == null)
        {
            Debug.LogWarning("[FloatingJoyStick] Canvas reference missing.");
            return;
        }

        var canvasRect = m_Canvas.transform as RectTransform;
        var cam = GetCanvasCamera();

        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, cam, out localPoint))
            localPoint = Vector2.zero;

        if (m_Canvas.scaleFactor != 0f)
            localPoint /= m_Canvas.scaleFactor;

        m_DragPos = localPoint;

        Vector2 delta = m_DragPos - m_PointerDownPos;
        delta = Vector2.ClampMagnitude(delta, m_MovementRange);

        if (m_Handle != null)
            m_Handle.anchoredPosition = delta;

        Vector2 newPos = new Vector2(delta.x / m_MovementRange, delta.y / m_MovementRange);
        SendValueToControl(newPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_Background != null)
            m_Background.gameObject.SetActive(false);

        if (m_Handle != null)
            m_Handle.anchoredPosition = Vector2.zero;

        SendValueToControl(Vector2.zero);
    }
}