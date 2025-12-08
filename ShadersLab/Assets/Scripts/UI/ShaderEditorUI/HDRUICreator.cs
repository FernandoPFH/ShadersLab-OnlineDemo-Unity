using UnityEngine;

[CreateAssetMenu(fileName = "HDRUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/HDRUICreator")]
public class HDRUICreator : ShaderEditorUICreator<HDRUIData>
{
    public override GameObject GenerateUIPerProperty(Material material, Shader shader, int propertyIndex)
    {
        GameObject ui = base.GenerateUIPerProperty(material, shader, propertyIndex);

        shaderUIDataHolders.Add(new HDRUIData(material, propertyIndex, material.GetColor(material.shader.GetPropertyNameId(propertyIndex)), ui));

        return ui;
    }
}

public class HDRUIData : ShaderUIData<Color>
{
    public HDRUIData(Material material, int propertyIndex, Color initialValue, GameObject ui) : base(material, propertyIndex, initialValue, ui)
    {
        ColorPickerUI colorPickerUI = ui.GetComponentInChildren<ColorPickerUI>();

        colorPickerUI.SetColor(initialValue);

        colorPickerUI.onValueChanged.AddListener((colorData) => material.SetColor(nameID, colorData.FullColor));
    }

    public override void ResetValue()
    {
        material.SetColor(nameID, defaultValue);

        UI.GetComponentInChildren<ColorPickerUI>().SetColor(defaultValue);
    }
}
