using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BoolUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/BoolUICreator")]
public class BoolUICreator : ShaderEditorUICreator<BoolUIData>
{
    public override GameObject GenerateUIPerAttribute(Material material, Shader shader, int propertyIndex, int attributeIndex)
    {
        GameObject ui = base.GenerateUIPerAttribute(material, shader, propertyIndex, attributeIndex);

        shaderUIDataHolders.Add(new BoolUIData(material, propertyIndex, attributeIndex, material.GetFloat(material.shader.GetPropertyNameId(propertyIndex)) > 0.5, ui));

        return ui;
    }
}

public class BoolUIData : ShaderUIData<bool>
{
    public BoolUIData(Material material, int propertyIndex, int attributeIndex, bool initialValue, GameObject ui) : base(material, propertyIndex, attributeIndex, initialValue, ui)
    {
        Toggle toggleField = ui.GetComponentInChildren<Toggle>();

        toggleField.isOn = initialValue;

        toggleField.onValueChanged.AddListener(delegate
        {
            material.SetFloat(nameID, toggleField.isOn ? 1f : 0f);
            lastValue = initialValue;
        });
    }

    public override void ResetValue()
    {
        material.SetFloat(nameID, defaultValue ? 1f : 0f);

        UI.GetComponentInChildren<Toggle>().isOn = defaultValue;
    }
}