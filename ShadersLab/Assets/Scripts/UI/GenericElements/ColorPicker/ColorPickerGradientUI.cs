using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ColorPickerGradientUI : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    [SerializeField] private RectTransform selectorKnob;
    [SerializeField] private Image grandientPreview;
    [SerializeField] private UnityEvent<Vector2> onValueChanged;

    public void OnPointerClick(PointerEventData eventData)
        => UpdateValue(eventData);

    public void OnDrag(PointerEventData eventData)
        => UpdateValue(eventData);

    private void UpdateValue(PointerEventData eventData)
    {
        Vector2 size = (transform as RectTransform).rect.size;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        float clampedX = localPoint.x < (-size.x / 2) ? (-size.x / 2) : localPoint.x;
        clampedX = localPoint.x > (size.x / 2) ? (size.x / 2) : clampedX;

        float clampedY = localPoint.y < (-size.y / 2) ? (-size.y / 2) : localPoint.y;
        clampedY = localPoint.y > (size.y / 2) ? (size.y / 2) : clampedY;

        localPoint = new(clampedX, clampedY);

        Vector2 SL = Vector2.one * 0.5f - (localPoint / -size);

        selectorKnob.anchoredPosition = localPoint - (size / 2f);

        onValueChanged.Invoke(SL);
    }

    public void SetSaturationLightness(Vector2 saturationLightness)
        => selectorKnob.anchoredPosition = (saturationLightness - Vector2.one) * (transform as RectTransform).rect.size * -1f;

    public void SetHue(float hue)
        => grandientPreview.material.SetFloat("_Hue", hue);
}
