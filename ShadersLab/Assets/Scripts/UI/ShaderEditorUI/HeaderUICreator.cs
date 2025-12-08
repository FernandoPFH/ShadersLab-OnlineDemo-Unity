using System.Linq;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "HeaderUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/HeaderUICreator")]
public class HeaderUICreator : ShaderEditorUICreator<HeaderUIData>
{
    public override GameObject GenerateUIPerAttribute(Material material, Shader shader, int propertyIndex, int attributeIndex)
    {
        GameObject ui = base.GenerateUIPerAttribute(material, shader, propertyIndex, attributeIndex);

        shaderUIDataHolders.Add(new HeaderUIData(material, propertyIndex, attributeIndex, 1, ui));

        return ui;
    }
}

public class HeaderUIData : ShaderUIData<int>
{
    public HeaderUIData(Material material, int propertyIndex, int attributeIndex, int initialValue, GameObject ui) : base(material, propertyIndex, attributeIndex, initialValue, ui)
    {
        ui.GetComponentInChildren<TextMeshProUGUI>().text = material.shader.GetPropertyAttributes(propertyIndex)[attributeIndex].Replace("Header(", "").Replace(")", "");
    }

    public override void ResetValue() { }
}