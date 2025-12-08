using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RangeUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/RangeUICreator")]
public class RangeUICreator : ShaderEditorUICreator<RangeUIData>
{
    public override GameObject GenerateUIPerProperty(Material material, Shader shader, int propertyIndex)
    {
        GameObject ui = base.GenerateUIPerProperty(material, shader, propertyIndex);

        shaderUIDataHolders.Add(new RangeUIData(material, propertyIndex, material.GetFloat(material.shader.GetPropertyNameId(propertyIndex)), ui));

        return ui;
    }
}

public class RangeUIData : ShaderUIData<float>
{
    public RangeUIData(Material material, int propertyIndex, float initialValue, GameObject ui) : base(material, propertyIndex, initialValue, ui)
    {
        Slider slider = ui.GetComponentInChildren<Slider>();
        TMP_InputField textfield = ui.GetComponentInChildren<TMP_InputField>();

        Vector2 rangeLimits = material.shader.GetPropertyRangeLimits(propertyIndex);
        slider.minValue = rangeLimits.x;
        slider.maxValue = rangeLimits.y;

        slider.value = initialValue;
        textfield.text = initialValue.ToString();

        slider.onValueChanged.AddListener(delegate
        {
            material.SetFloat(nameID, slider.value);
            textfield.text = slider.value.ToString();
        });

        textfield.onValueChanged.AddListener(delegate
        {
            if (float.TryParse(textfield.text.Replace(".", ","), out float parsedValue))
            {
                material.SetFloat(nameID, parsedValue);
                slider.value = parsedValue;
            }
            else
                textfield.text = lastValue.ToString();
        });
    }

    public override void ResetValue()
    {
        material.SetFloat(nameID, defaultValue);

        UI.GetComponentInChildren<Slider>().value = defaultValue;
        UI.GetComponentInChildren<TMP_InputField>().text = defaultValue.ToString();
    }
}
