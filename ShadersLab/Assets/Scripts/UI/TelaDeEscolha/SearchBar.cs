using UnityEngine;

public class SearchBar : MonoBehaviour
{
    [SerializeField] private FiltersBar filtersBar;

    public bool IsOpen { get; private set; }

    public void OpenUI()
    {
        RectTransform rectTransform = transform as RectTransform;

        rectTransform.anchoredPosition = new(rectTransform.anchoredPosition.x, -(rectTransform.rect.height + SearchBarSettings.SpacingBetweenTopBar));
        TelaDeEscolha.SetGridTop(rectTransform.rect.height * 2 + SearchBarSettings.SpacingBetweenTopBar * 2);

        IsOpen = true;
    }

    public void CloseUI()
    {
        filtersBar.CloseUI();

        RectTransform rectTransform = transform as RectTransform;

        rectTransform.anchoredPosition = new(rectTransform.anchoredPosition.x, 0f);
        TelaDeEscolha.SetGridTop(rectTransform.rect.height);

        IsOpen = false;
    }

    public void UpdateTextFilter(string search)
        => TelaDeEscolha.ChangeSearch(search);
}
