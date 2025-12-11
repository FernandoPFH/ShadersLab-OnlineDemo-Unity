using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ColorPickerWheelUI : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    [SerializeField] private RectTransform selectorKnob;
    [SerializeField] private UnityEvent<float> onValueChanged;

    private float colorKnobDistance;
    private float colorKnobSize;

    void Awake()
    {
        colorKnobDistance = selectorKnob.anchoredPosition.magnitude;
        colorKnobSize = selectorKnob.rect.width;
    }

    public void OnPointerClick(PointerEventData eventData)
        => UpdateValue(eventData, true);

    public void OnDrag(PointerEventData eventData)
        => UpdateValue(eventData);

    private void UpdateValue(PointerEventData eventData, bool needToCheckDistance = false)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        localPoint += (transform as RectTransform).sizeDelta / 2;

        if (needToCheckDistance)
        {
            float distancePoint = localPoint.magnitude;

            if (distancePoint < colorKnobDistance - colorKnobSize / 2 || distancePoint > colorKnobDistance + colorKnobSize / 2)
                return;
        }

        selectorKnob.anchoredPosition = localPoint.normalized * colorKnobDistance;

        float angle = Vector2.SignedAngle(new(1f, 0f), localPoint.normalized);

        if (angle < 0f)
            angle += 360f;

        onValueChanged.Invoke(angle / 360f);
    }

    public void SetHue(float hue)
        => selectorKnob.anchoredPosition = new Vector2(Mathf.Cos(hue * 360f * Mathf.Deg2Rad), Mathf.Sin(hue * 360f * Mathf.Deg2Rad)) * colorKnobDistance;
}
