using UnityEngine;
using UnityEngine.UI;
using Unity.VectorGraphics;
using TMPro;

public class ItemDoGrid : MonoBehaviour
{
    [SerializeField] private Image backgroundHolder;
    [SerializeField] private TextMeshProUGUI titleHolder;
    [SerializeField] private SVGImage typeIconHolder;
    [SerializeField] private Animator animator;

    public ShaderInfos shaderInfos { get; private set; }
    private float lastPressTime;

    void OnValidate()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    public void Initialize(ShaderInfos shaderInfos)
    {
        this.shaderInfos = shaderInfos;

        backgroundHolder.sprite = shaderInfos.MainImage;

        titleHolder.text = shaderInfos.Nome;

        typeIconHolder.sprite = shaderInfos.Tipo.Icone;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        backgroundHolder.raycastTarget = true;
        animator.Play("Base Layer.Aparecer");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        backgroundHolder.raycastTarget = false;
        animator.Play("Base Layer.Desaparecer");
    }

    public void HandlePress()
    {
        if (Time.time - lastPressTime < 0.4f)
        {
            SceneTransitionManager.OpenShaderDisplayScene(shaderInfos);
            return;
        }

        lastPressTime = Time.time;

        TelaDeEscolha.OpenShaderSummary(shaderInfos);
    }
}
