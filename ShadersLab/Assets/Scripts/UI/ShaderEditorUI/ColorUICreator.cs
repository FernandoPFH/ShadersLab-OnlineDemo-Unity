using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/ColorUICreator")]
public class ColorUICreator : ShaderEditorUICreator<ColorUIData>
{
    public override GameObject GenerateUI(Material material, Shader shader, int propertyIndex)
    {
        GameObject ui = base.GenerateUI(material, shader, propertyIndex);

        shaderUIDataHolders.Add(new ColorUIData(material, propertyIndex, material.GetColor(material.shader.GetPropertyNameId(propertyIndex)), ui));

        return ui;
    }
}

public class ColorUIData : ShaderUIData<Color>
{
    public ColorUIData(Material material, int propertyIndex, Color initialValue, GameObject ui) : base(material, propertyIndex, initialValue, ui)
    {
        TMP_InputField[] textfields = ui.GetComponentsInChildren<TMP_InputField>();

        for (int i = 0; i < textfields.Length; i++)
        {
            TMP_InputField textfield = textfields[i];
            textfield.text = initialValue[i].ToString();
            int colorIndex = i;

            textfield.onValueChanged.AddListener(delegate
            {
                if (float.TryParse(textfield.text.Replace(".", ","), out float parsedValue) && parsedValue >= 0f && parsedValue <= 1f)
                {
                    lastValue[colorIndex] = parsedValue;
                    material.SetColor(nameID, lastValue);
                }
                else
                    textfield.text = lastValue[i].ToString();
            });
        }
    }

    public override void ResetValue()
    {
        material.SetColor(nameID, defaultValue);

        TMP_InputField[] textfields = UI.GetComponentsInChildren<TMP_InputField>();

        for (int i = 0; i < textfields.Length; i++)
            textfields[i].text = defaultValue[i].ToString();
    }
}
