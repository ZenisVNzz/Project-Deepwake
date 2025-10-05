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

        m_Background.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        if (eventData.position.x > Screen.width / 2f)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            eventData.position,
            m_Canvas.worldCamera,
            out m_PointerDownPos);

        m_Background.anchoredPosition = m_PointerDownPos;
        m_Handle.anchoredPosition = Vector2.zero;
        m_Background.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        if (eventData.position.x > Screen.width / 2f)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_Canvas.transform as RectTransform,
            eventData.position,
            m_Canvas.worldCamera,
            out m_DragPos);

        Vector2 delta = m_DragPos - m_PointerDownPos;
        delta = Vector2.ClampMagnitude(delta, m_MovementRange);

        m_Handle.anchoredPosition = delta;

        Vector2 newPos = new Vector2(delta.x / m_MovementRange, delta.y / m_MovementRange);
        SendValueToControl(newPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_Background.gameObject.SetActive(false);
        m_Handle.anchoredPosition = Vector2.zero;
        SendValueToControl(Vector2.zero);
    }
}