using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiltersBar : MonoBehaviour
{
    [SerializeField] private GameObject filterButtonPrefab;

    private List<FilterButton> filterButtonCriados = new();

    public bool IsOpen { get; private set; }

    void Start()
    {
        foreach (TipoInfos tipo in TipoInfos.Instances.Values)
            CreateItem(tipo);
    }

    void CreateItem(TipoInfos tipo)
    {
        FilterButton filterButton = Instantiate(filterButtonPrefab, transform).GetComponent<FilterButton>();

        filterButton.Initialize(tipo);

        filterButtonCriados.Add(filterButton);
    }

    public void HandlePress()
    {
        if (IsOpen)
            CloseUI();
        else
            OpenUI();
    }

    public void OpenUI()
    {
        RectTransform rectTransform = transform as RectTransform;

        rectTransform.anchoredPosition = new(rectTransform.anchoredPosition.x, -(rectTransform.rect.height + FilterBarSettings.SpacingBetweenSearchBar));
        TelaDeEscolha.SetGridTop(rectTransform.rect.height * 3 + SearchBarSettings.SpacingBetweenTopBar + FilterBarSettings.SpacingBetweenSearchBar * 2);

        IsOpen = true;

        gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        RectTransform rectTransform = transform as RectTransform;

        rectTransform.anchoredPosition = new(rectTransform.anchoredPosition.x, 0f);
        TelaDeEscolha.SetGridTop(rectTransform.rect.height * 2 + SearchBarSettings.SpacingBetweenTopBar * 2);

        IsOpen = false;

        gameObject.SetActive(false);
    }
}
