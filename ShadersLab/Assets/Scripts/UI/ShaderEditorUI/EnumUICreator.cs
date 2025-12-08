using System.Linq;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "EnumUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/EnumUICreator")]
public class EnumUICreator : ShaderEditorUICreator<EnumUIData>
{
    public override GameObject GenerateUIPerAttribute(Material material, Shader shader, int propertyIndex, int attributeIndex)
    {
        GameObject ui = base.GenerateUIPerAttribute(material, shader, propertyIndex, attributeIndex);

        shaderUIDataHolders.Add(new EnumUIData(material, propertyIndex, attributeIndex, material.GetFloat(material.shader.GetPropertyNameId(propertyIndex)), ui));

        return ui;
    }
}

public class EnumUIData : ShaderUIData<float>
{
    public EnumUIData(Material material, int propertyIndex, int attributeIndex, float initialValue, GameObject ui) : base(material, propertyIndex, attributeIndex, initialValue, ui)
    {
        TMP_Dropdown dropdown = ui.GetComponentInChildren<TMP_Dropdown>();

        dropdown.AddOptions(material.shader.GetPropertyAttributes(propertyIndex)[attributeIndex].Replace("KeywordEnum(", "").Replace(")", "").Split(",").ToList());

        dropdown.value = Mathf.FloorToInt(defaultValue);

        dropdown.onValueChanged.AddListener(delegate
        {
            material.SetFloat(nameID, dropdown.value);
            lastValue = dropdown.value;
        });
    }

    public override void ResetValue()
    {
        material.SetFloat(nameID, defaultValue);

        UI.GetComponentInChildren<TMP_Dropdown>().value = Mathf.FloorToInt(defaultValue);
    }
}