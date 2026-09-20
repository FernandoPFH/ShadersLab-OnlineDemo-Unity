using UnityEngine;
using UnityEngine.UI;

public class ColapsableUI : MonoBehaviour
{
    [SerializeField] private GameObject header;

    private float openHeight;
    private float closeHeight;
    private RectTransform rectTransform;


    void Start()
    {
        rectTransform = transform as RectTransform;

        VerticalLayoutGroup verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();

        openHeight = rectTransform.sizeDelta.y;
        closeHeight = header.GetComponent<LayoutElement>().minHeight + verticalLayoutGroup.padding.top + verticalLayoutGroup.padding.bottom;

        rectTransform.sizeDelta = new(rectTransform.sizeDelta.x, closeHeight);
    }

    public void HandleToggle(bool isOn)
        => rectTransform.sizeDelta = isOn ? new(rectTransform.sizeDelta.x, openHeight) : new(rectTransform.sizeDelta.x, closeHeight);
}
