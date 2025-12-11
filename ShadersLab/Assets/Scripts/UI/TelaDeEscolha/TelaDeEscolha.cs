using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

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
        List<ShaderInfos> shaderInfos = ShaderInfos.Instances.ToList();
        shaderInfos.Sort((a, b) => a.Nome.CompareTo(b.Nome));

        foreach (ShaderInfos shaderInfo in shaderInfos)
            CriarItem(shaderInfo);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            if (searchBarHolder.IsOpen)
                searchBarHolder.CloseUI();
            else
                searchBarHolder.OpenUI();
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

    public static ShaderSummary ShaderSummary => Instance.shaderSummaryHolder;

    public static void SetGridTop(float top)
        => (Instance.conteudoHolder as RectTransform).offsetMax = new((Instance.conteudoHolder as RectTransform).offsetMax.x, -top);

    private TipoInfos typeFilter;
    private string searchFilter = "";

    public static void ChangeFilter(TipoInfos type)
    {
        Instance.typeFilter = type;

        FilterShaders();
    }

    public static void ChangeSearch(string search)
    {
        Instance.searchFilter = search;

        FilterShaders();
    }

    public static void FilterShaders()
    {
        foreach (ItemDoGrid itemDoGrid in Instance.itensDoGridCriados)
            if ((!Instance.typeFilter || itemDoGrid.shaderInfos.Tipo == Instance.typeFilter) && Regex.IsMatch(itemDoGrid.shaderInfos.Nome, Instance.searchFilter))
                itemDoGrid.Show();
            else
                itemDoGrid.Hide();
    }

    public void OpenShaderDisplayScene()
        => SceneTransitionManager.OpenShaderDisplayScene(shaderSummaryHolder.CurrentShaderInfos);

    public void HandleBackPress()
    {
        searchBarHolder.CloseUI();
        shaderSummaryHolder.CloseUI();
    }
}