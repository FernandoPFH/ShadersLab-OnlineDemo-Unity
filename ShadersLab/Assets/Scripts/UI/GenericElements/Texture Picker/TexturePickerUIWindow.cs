using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TexturePickerUIWindow : MonoBehaviour, IDragHandler
{
    [SerializeField] private GameObject showHideTextures;
    [SerializeField] private Transform contentHolder;
    [SerializeField] private GameObject listOfTexturesPrefab;
    public UnityEvent<Texture2D> onValueChanged = new();
    private bool isShowingAllTexture;
    private ShaderInfos CurrentShaderInfos;

    public void SetupUI(Vector2 screenPosition, ShaderInfos shaderInfos)
    {
        transform.SetParent(FindFirstObjectByType<Canvas>().transform);
        (transform as RectTransform).anchoredPosition = screenPosition;

        foreach (Transform child in contentHolder)
            Destroy(child.gameObject);

        CurrentShaderInfos = shaderInfos;

        showHideTextures.GetComponentInChildren<TextMeshProUGUI>().text = "Show All";
        isShowingAllTexture = false;
        showHideTextures.SetActive(shaderInfos.texturesToShow.Count != 0);

        foreach (string textureListName in TexturePickerSettings.ListsOfTextures.Keys.ToList())
        {
            GameObject listOfTexture = Instantiate(listOfTexturesPrefab, contentHolder);

            listOfTexture.GetComponent<TextureScrollList>().SetupUI(textureListName, TexturePickerSettings.ListsOfTextures[textureListName], SetTexture);

            listOfTexture.SetActive(shaderInfos.texturesToShow.Count == 0 || shaderInfos.texturesToShow.Contains(textureListName));
        }

        gameObject.SetActive(true);
    }

    public void ToggleTextureVisibility()
    {
        if (isShowingAllTexture)
        {
            showHideTextures.GetComponentInChildren<TextMeshProUGUI>().text = "Show All";

            foreach (Transform child in contentHolder)
                child.gameObject.SetActive(CurrentShaderInfos.texturesToShow.Contains(child.gameObject.GetComponentInChildren<TextMeshProUGUI>().text));
        }
        else
        {
            showHideTextures.GetComponentInChildren<TextMeshProUGUI>().text = "Hide All";

            foreach (Transform child in contentHolder)
                child.gameObject.SetActive(true);
        }

        isShowingAllTexture = !isShowingAllTexture;
    }

    public void SetTexture(Texture2D texture2D)
        => onValueChanged.Invoke(texture2D);

    public void OnDrag(PointerEventData eventData)
        => (transform as RectTransform).anchoredPosition += eventData.delta;

    public void CloseWindow()
    {
        gameObject.SetActive(false);
        onValueChanged = new();
    }
}