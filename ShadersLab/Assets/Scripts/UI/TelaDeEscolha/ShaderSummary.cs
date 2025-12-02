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

    public void OpenUI(ShaderInfos shaderInfos)
    {
        RectTransform rectTransform = transform as RectTransform;

        rectTransform.anchoredPosition = new(-rectTransform.rect.width, rectTransform.anchoredPosition.y);
        TelaDeEscolha.SetGridRight(rectTransform.rect.width);

        backgroundHolder.sprite = shaderInfos.MainImage;

        titleHolder.text = shaderInfos.Nome;

        typeTextHolder.text = shaderInfos.Tipo.Nome;
        typeIconHolder.sprite = shaderInfos.Tipo.Icone;

        itemGridDefaultHorizontalPadding = TelaDeEscolha.GetGridHorizontalPadding();
        TelaDeEscolha.SetGridHorizontalPadding(Vector2Int.one * ItemGridSettings.HorizontalPaddingWhenCompact);

        IsOpen = true;
    }

    public void CloseUI()
    {
        RectTransform rectTransform = transform as RectTransform;

        rectTransform.anchoredPosition = new(0f, rectTransform.anchoredPosition.y);
        TelaDeEscolha.SetGridRight(0f);

        TelaDeEscolha.SetGridHorizontalPadding(itemGridDefaultHorizontalPadding);

        IsOpen = false;
    }
}
