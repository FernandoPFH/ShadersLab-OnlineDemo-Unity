using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "VectorUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/VectorUICreator")]
public class VectorUICreator : ShaderEditorUICreator<VectorUIData>
{
    public override GameObject GenerateUIPerProperty(Material material, Shader shader, int propertyIndex)
    {
        GameObject ui = base.GenerateUIPerProperty(material, shader, propertyIndex);

        shaderUIDataHolders.Add(new VectorUIData(material, propertyIndex, material.GetVector(material.shader.GetPropertyNameId(propertyIndex)), ui));

        return ui;
    }
}

public class VectorUIData : ShaderUIData<Vector4>
{
    public VectorUIData(Material material, int propertyIndex, Vector4 initialValue, GameObject ui) : base(material, propertyIndex, initialValue, ui)
    {
        TMP_InputField[] textfields = ui.GetComponentsInChildren<TMP_InputField>();

        for (int i = 0; i < textfields.Length; i++)
        {
            TMP_InputField textfield = textfields[i];
            textfield.text = initialValue[i].ToString();
            int vectorIndex = i;

            textfield.onValueChanged.AddListener(delegate
            {
                if (float.TryParse(textfield.text.Replace(".", ","), out float parsedValue))
                {
                    lastValue[vectorIndex] = parsedValue;
                    material.SetVector(nameID, lastValue);
                }
                else
                    textfield.text = lastValue[vectorIndex].ToString();
            });
        }
    }

    public override void ResetValue()
    {
        material.SetVector(nameID, defaultValue);

        TMP_InputField[] textfields = UI.GetComponentsInChildren<TMP_InputField>();

        for (int i = 0; i < textfields.Length; i++)
            textfields[i].text = defaultValue[i].ToString();
    }
}