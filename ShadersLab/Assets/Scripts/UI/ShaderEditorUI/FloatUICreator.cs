using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/FloatUICreator")]
public class FloatUICreator : ShaderEditorUICreator<FloatUIData>
{
    public override GameObject GenerateUI(Material material, Shader shader, int propertyIndex)
    {
        GameObject ui = base.GenerateUI(material, shader, propertyIndex);

        shaderUIDataHolders.Add(new FloatUIData(material, propertyIndex, material.GetFloat(material.shader.GetPropertyNameId(propertyIndex)), ui));

        return ui;
    }
}

public class FloatUIData : ShaderUIData<float>
{
    public FloatUIData(Material material, int propertyIndex, float initialValue, GameObject ui) : base(material, propertyIndex, initialValue, ui)
    {
        TMP_InputField textfield = ui.GetComponentInChildren<TMP_InputField>();

        textfield.text = initialValue.ToString();

        textfield.onValueChanged.AddListener(delegate
        {
            if (float.TryParse(textfield.text.Replace(".", ","), out float parsedValue))
            {
                material.SetFloat(nameID, parsedValue);
                lastValue = parsedValue;
            }
            else
                textfield.text = lastValue.ToString();
        });
    }

    public override void ResetValue()
    {
        material.SetFloat(nameID, defaultValue);

        UI.GetComponentInChildren<TMP_InputField>().text = defaultValue.ToString();
    }
}