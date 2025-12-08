using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;
using TMPro;

public class ShaderSummary : MonoBehaviour
{
    [SerializeField] private Image backgroundHolder;
    [SerializeField] private TextMeshProUGUI titleHolder;
    [SerializeField] private TextMeshProUGUI typeTextHolder;
    [SerializeField] private SVGImage typeIconHolder;

    public bool IsOpen;

    private Vector2Int itemGridDefaultHorizontalPadding;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsOpen)
            CloseUI();
    }

    public void OpenUI(ShaderInfos shaderInfos)
    {
        RectTransform rectTransform = transform as RectTransform;

        rectTransform.anchoredPosition = new(-rectTransform.rect.width, rectTransform.anchoredPosition.y);

        backgroundHolder.sprite = shaderInfos.MainImage;

        titleHolder.text = shaderInfos.Nome;

        typeTextHolder.text = shaderInfos.Tipo.Nome;
        typeIconHolder.sprite = shaderInfos.Tipo.Icone;

        IsOpen = true;
    }

    public void CloseUI()
    {
        RectTransform rectTransform = transform as RectTransform;

        rectTransform.anchoredPosition = new(0f, rectTransform.anchoredPosition.y);

        IsOpen = false;
    }
}
