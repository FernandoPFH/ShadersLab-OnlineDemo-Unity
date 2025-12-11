using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class LabelDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_InputField inputField;

    public static bool IsBeingDragged;

    public void OnDrag(PointerEventData eventData)
    {
        if (float.TryParse(inputField.text, out float parsedText))
        {
            parsedText += eventData.delta.x * LabelDraggerSettings.DragMultiplier;

            inputField.text = parsedText.ToString();

            inputField.onValueChanged.Invoke(inputField.text);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
        => IsBeingDragged = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        IsBeingDragged = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerEnter(PointerEventData eventData)
        => Cursor.SetCursor(LabelDraggerSettings.DragCursorTexture, LabelDraggerSettings.DragCursorTexture ? new Vector2(LabelDraggerSettings.DragCursorTexture.width, LabelDraggerSettings.DragCursorTexture.height) / 2f : Vector2.zero, CursorMode.Auto);

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsBeingDragged)
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}