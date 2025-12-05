using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BoolUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/BoolUICreator")]
public class BoolUICreator : ShaderEditorUICreator<BoolUIData>
{
    public override GameObject GenerateUI(Material material, Shader shader, int propertyIndex)
    {
        GameObject ui = base.GenerateUI(material, shader, propertyIndex);

        shaderUIDataHolders.Add(new BoolUIData(material, propertyIndex, material.GetFloat(material.shader.GetPropertyNameId(propertyIndex)) > 0.5, ui));

        return ui;
    }
}

public class BoolUIData : ShaderUIData<bool>
{
    public BoolUIData(Material material, int propertyIndex, bool initialValue, GameObject ui) : base(material, propertyIndex, initialValue, ui)
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