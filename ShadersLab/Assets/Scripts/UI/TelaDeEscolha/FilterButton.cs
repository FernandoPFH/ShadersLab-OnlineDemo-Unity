using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class FilterButton : MonoBehaviour
{
    [SerializeField] private SVGImage iconHolder;
    [SerializeField] private TextMeshProUGUI titleHolder;
    [SerializeField] private Button button;

    public TipoInfos tipo { get; private set; }

    private bool IsSelected;
    private Color defaultButtonColor;

    public static HashSet<FilterButton> Instances = new();

    private void Awake()
    {
        Instances.Add(this);
    }

    public void Initialize(TipoInfos tipo)
    {
        this.tipo = tipo;

        iconHolder.sprite = tipo.Icone;

        titleHolder.text = tipo.Nome;

        defaultButtonColor = button.targetGraphic.color;
    }

    void OnValidate()
    {
        if (!button) button = GetComponent<Button>();
    }

    public void HandlePress()
    {
        if (IsSelected)
        {
            TelaDeEscolha.ChangeFilter(null);
            button.targetGraphic.color = defaultButtonColor;
        }
        else
        {
            TelaDeEscolha.ChangeFilter(tipo);
            button.targetGraphic.color = button.colors.disabledColor;
        }

        IsSelected = !IsSelected;
    }

    public void ResetPress()
    {
        IsSelected = false;
        button.targetGraphic.color = defaultButtonColor;
    }
}
