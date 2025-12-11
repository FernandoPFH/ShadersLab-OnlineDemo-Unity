using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using Unity.VisualScripting;

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

        float clampedX = localPoint.x < 0f ? 0f : localPoint.x;
        clampedX = clampedX > size.x ? size.x : clampedX;

        float clampedY = localPoint.y < 0f ? 0f : localPoint.y;
        clampedY = clampedY > size.y ? size.y : clampedY;

        localPoint = new(clampedX, clampedY);

        selectorKnob.anchoredPosition = localPoint;

        onValueChanged.Invoke(localPoint / size);
    }

    public void SetSaturationLightness(Vector2 saturationLightness)
        => selectorKnob.anchoredPosition = saturationLightness * (transform as RectTransform).rect.size.Abs();

    public void SetHue(float hue)
        => grandientPreview.material.SetFloat("_Hue", hue);
}
