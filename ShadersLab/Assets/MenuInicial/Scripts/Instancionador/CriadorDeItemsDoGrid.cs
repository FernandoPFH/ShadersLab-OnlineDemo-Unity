using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CriadorDeItemsDoGrid : MonoBehaviour
{
    [SerializeField] private GameObject itemDoGridPrefab;

    [SerializeField] private List<ShaderInfos> shadersInfos;

    private List<GameObject> itensDoGridCriados;

    void Awake()
    {
        itensDoGridCriados = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (ShaderInfos shaderInfo in shadersInfos) {
            CriarItem(shaderInfo);
        }
    }

    void CriarItem(ShaderInfos shaderInfo) {
        GameObject itemDoGrid = Instantiate(itemDoGridPrefab, transform);

        GameObject background = itemDoGrid.transform.GetChild(0).gameObject;

        background.GetComponent<Image>().sprite = shaderInfo.MainImage;

        GameObject titulo = background.transform.GetChild(1).gameObject;

        titulo.GetComponent<TextMeshProUGUI>().text = shaderInfo.Nome;

        GameObject icone = background.transform.GetChild(2).gameObject;

        icone.GetComponent<Unity.VectorGraphics.SVGImage>().sprite = shaderInfo.Tipo.Icone;

        itemDoGrid.SetActive(false);

        itensDoGridCriados.Add(itemDoGrid);
    }

    public void AparecerItens() {
        foreach (GameObject itemDoGrid in itensDoGridCriados) {
            itemDoGrid.SetActive(true);
            itemDoGrid.transform.GetChild(0).gameObject.GetComponent<Image>().raycastTarget = true;
            itemDoGrid.GetComponent<Animator>().Play("Base Layer.Aparecer");
        }
    }

    public void DesaparecerItens() {
        foreach (GameObject itemDoGrid in itensDoGridCriados) {
            itemDoGrid.transform.GetChild(0).gameObject.GetComponent<Image>().raycastTarget = false;
            itemDoGrid.GetComponent<Animator>().Play("Base Layer.Desaparecer");
        }
    }

    public void DesativarItens(){
        if (itensDoGridCriados.Count > 0) {
            foreach (GameObject itemDoGrid in itensDoGridCriados) {
                itemDoGrid.SetActive(false);
            }
        }
    }
}