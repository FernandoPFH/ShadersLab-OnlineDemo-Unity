using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "BoolUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/BoolUICreator")]
public class BoolUICreator : ShaderEditorUICreator<BoolUIData>
{
    public override GameObject GenerateUIPerAttribute(Material material, Shader shader, int propertyIndex, int attributeIndex)
    {
        GameObject ui = base.GenerateUIPerAttribute(material, shader, propertyIndex, attributeIndex);

        shaderUIDataHolders.Add(new BoolUIData(material, propertyIndex, attributeIndex, material.GetFloat(material.shader.GetPropertyNameId(propertyIndex)) > 0.5, ui));

        return ui;
    }

    public override GameObject GenerateUIPerProperty<T>(string label, Action<T> onChange, T defaultValue)
    {
        GameObject ui = base.GenerateUIPerProperty(label, onChange, defaultValue);

        if (defaultValue is bool defaultBoolValue)
            shaderUIDataHolders.Add(new BoolUIData(label, onChange as Action<bool>, defaultBoolValue, ui));
        else
            Debug.LogError($"Bool UI Creator: Wrong type of defaultValue of {label}!");

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

    public BoolUIData(string label, Action<bool> onChange, bool defaultValue, GameObject ui) : base(defaultValue, ui)
    {
        Toggle toggleField = ui.GetComponentInChildren<Toggle>();

        toggleField.isOn = defaultValue;

        toggleField.onValueChanged.AddListener((isToggled) => { onChange(isToggled); });
    }

    public override void ResetValue()
    {
        if (material) material.SetFloat(nameID, defaultValue ? 1f : 0f);

        UI.GetComponentInChildren<Toggle>().isOn = defaultValue;
    }
}
