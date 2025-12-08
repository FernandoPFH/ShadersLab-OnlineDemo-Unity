using UnityEngine;

[CreateAssetMenu(fileName = "ColorUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/ColorUICreator")]
public class ColorUICreator : ShaderEditorUICreator<ColorUIData>
{
    public override GameObject GenerateUIPerProperty(Material material, Shader shader, int propertyIndex)
    {
        GameObject ui = base.GenerateUIPerProperty(material, shader, propertyIndex);

        shaderUIDataHolders.Add(new ColorUIData(material, propertyIndex, material.GetColor(material.shader.GetPropertyNameId(propertyIndex)), ui));

        return ui;
    }
}

public class ColorUIData : ShaderUIData<Color>
{
    public ColorUIData(Material material, int propertyIndex, Color initialValue, GameObject ui) : base(material, propertyIndex, initialValue, ui)
    {
        ColorPickerUI colorPickerUI = ui.GetComponentInChildren<ColorPickerUI>();

        colorPickerUI.SetColor(initialValue);
        colorPickerUI.HideHDRLabel();

        colorPickerUI.onValueChanged.AddListener((colorData) => material.SetColor(nameID, colorData.FullColor));
    }

    public override void ResetValue()
    {
        material.SetColor(nameID, defaultValue);

        UI.GetComponentInChildren<ColorPickerUI>().SetColor(defaultValue);
    }
}
