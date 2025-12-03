using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelaDeEscolha : Singleton<TelaDeEscolha>
{
    [SerializeField] private GameObject itemDoGridPrefab;
    [SerializeField] private Transform conteudoHolder;
    [SerializeField] private Transform fullGrid;
    [SerializeField] private Transform grid;
    [SerializeField] private ShaderSummary shaderSummaryHolder;
    [SerializeField] private SearchBar searchBarHolder;
    private List<ItemDoGrid> itensDoGridCriados = new List<ItemDoGrid>();

    void Start()
    {
        foreach (ShaderInfos shaderInfo in ShaderInfos.Instances)
            CriarItem(shaderInfo);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            if (searchBarHolder.IsOpen)
                searchBarHolder.CloseUI();
            else
                searchBarHolder.OpenUI();

        if (Input.GetKeyDown(KeyCode.Escape) && shaderSummaryHolder.IsOpen)
            shaderSummaryHolder.CloseUI();
    }

    void CriarItem(ShaderInfos shaderInfo)
    {
        ItemDoGrid itemDoGrid = Instantiate(itemDoGridPrefab, grid).GetComponent<ItemDoGrid>();

        itemDoGrid.Initialize(shaderInfo);

        itemDoGrid.gameObject.SetActive(false);

        itensDoGridCriados.Add(itemDoGrid);
    }

    public void AparecerItens()
    {
        foreach (ItemDoGrid itemDoGrid in itensDoGridCriados)
            itemDoGrid.Show();
    }

    public void DesaparecerItens()
    {
        foreach (ItemDoGrid itemDoGrid in itensDoGridCriados)
            itemDoGrid.Hide();
    }

    public void DesativarItens()
    {
        foreach (ItemDoGrid itemDoGrid in itensDoGridCriados)
            itemDoGrid.gameObject.SetActive(false);
    }

    public static void OpenShaderSummary(ShaderInfos shaderInfo)
        => Instance.shaderSummaryHolder.OpenUI(shaderInfo);

    public static void CloseShaderSummary()
        => Instance.shaderSummaryHolder.CloseUI();

    public static ShaderSummary ShaderSummary => Instance.shaderSummaryHolder;

    public static void SetGridRight(float right)
        => (Instance.fullGrid as RectTransform).offsetMax = new(-right, (Instance.fullGrid as RectTransform).offsetMax.y);

    public static void SetGridTop(float top)
        => (Instance.conteudoHolder as RectTransform).offsetMax = new((Instance.conteudoHolder as RectTransform).offsetMax.x, -top);

    public static Vector2Int GetGridHorizontalPadding()
        => new(Instance.grid.GetComponent<GridLayoutGroup>().padding.left, Instance.grid.GetComponent<GridLayoutGroup>().padding.right);

    public static void SetGridHorizontalPadding(Vector2Int horizontalPadding)
    {
        Instance.grid.GetComponent<GridLayoutGroup>().padding.left = (int)horizontalPadding.x;
        Instance.grid.GetComponent<GridLayoutGroup>().padding.right = (int)horizontalPadding.y;
    }
    private TipoInfos typeFilter;

    public static void ChangeFilter(TipoInfos type)
    {
        Instance.typeFilter = type;

        FilterShaders();
    }

    public static void FilterShaders()
    {
        foreach (FilterButton filterButton in FilterButton.Instances)
            filterButton.ResetPress();

        if (Instance.typeFilter)
            foreach (ItemDoGrid itemDoGrid in Instance.itensDoGridCriados)
            {
                if (itemDoGrid.shaderInfos.Tipo == Instance.typeFilter)
                    itemDoGrid.Show();
                else
                    itemDoGrid.Hide();
            }
        else
            foreach (ItemDoGrid itemDoGrid in Instance.itensDoGridCriados)
                itemDoGrid.Show();
    }

    public void HandleBackPress()
    {
        searchBarHolder.CloseUI();
        shaderSummaryHolder.CloseUI();
    }
}