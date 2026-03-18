using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "IntRangeUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/IntRangeUICreator")]
public class IntRangeUICreator : ShaderEditorUICreator<IntRangeUIData>
{
    public override GameObject GenerateUIPerAttribute(Material material, Shader shader, int propertyIndex, int attributeIndex)
    {
        GameObject ui = base.GenerateUIPerProperty(material, shader, propertyIndex);

        shaderUIDataHolders.Add(new IntRangeUIData(material, propertyIndex, attributeIndex, material.GetInt(material.shader.GetPropertyNameId(propertyIndex)), ui));

        return ui;
    }
}

public class IntRangeUIData : ShaderUIData<int>
{
    public IntRangeUIData(Material material, int propertyIndex, int attributeIndex, int initialValue, GameObject ui) : base(material, propertyIndex, attributeIndex, initialValue, ui)
    {
        Slider slider = ui.GetComponentInChildren<Slider>();
        TMP_InputField textfield = ui.GetComponentInChildren<TMP_InputField>();

        Vector2 rangeLimits = material.shader.GetPropertyRangeLimits(propertyIndex);
        slider.minValue = Mathf.Floor(rangeLimits.x);
        slider.maxValue = Mathf.Floor(rangeLimits.y);

        slider.value = initialValue;
        textfield.text = initialValue.ToString();

        slider.onValueChanged.AddListener(delegate
        {
            material.SetFloat(nameID, slider.value);
            textfield.text = slider.value.ToString();
        });

        textfield.onValueChanged.AddListener(delegate
        {
            if (int.TryParse(textfield.text.Replace(".", ","), out int parsedValue))
            {
                material.SetInt(nameID, parsedValue);
                slider.value = parsedValue;
            }
            else
                textfield.text = lastValue.ToString();
        });
    }

    public override void ResetValue()
    {
        material.SetInt(nameID, defaultValue);

        UI.GetComponentInChildren<Slider>().value = defaultValue;
        UI.GetComponentInChildren<TMP_InputField>().text = defaultValue.ToString();
    }
}
