using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TextureUICreator", menuName = "ScriptableObjects/UI/ShaderEditorUI/Creators/TextureUICreator")]
public class TextureUICreator : ShaderEditorUICreator<TextureUIData>
{
    private Dictionary<Type, Func<Texture, Texture2D>> casters = new()
    {
        {typeof(Texture2D) , (texture) => texture as Texture2D},
    };

    public override GameObject GenerateUI(Material material, Shader shader, int propertyIndex)
    {
        Texture texture = material.GetTexture(material.shader.GetPropertyNameId(propertyIndex));

        Texture2D castedTexture = null;
        foreach (Type type in casters.Keys)
        {
            if (texture.GetType() == type)
            {
                castedTexture = casters[type](texture);
                break;
            }
        }

        if (!castedTexture)
        {
            Debug.Log($"No caster for type: {material.GetTexture(material.shader.GetPropertyNameId(propertyIndex)).GetType()}");
            return new();
        }

        GameObject ui = base.GenerateUI(material, shader, propertyIndex);

        shaderUIDataHolders.Add(new TextureUIData(material, propertyIndex, castedTexture, ui));

        return ui;
    }
}

public class TextureUIData : ShaderUIData<Texture2D>
{
    private (Vector2, Action<int, Vector2>)[] texturesDefaultData = { };
    private Vector2[] texturesLastData = { };

    public TextureUIData(Material material, int propertyIndex, Texture2D initialValue, GameObject ui) : base(material, propertyIndex, initialValue, ui)
    {
        TMP_InputField[] textfields = ui.GetComponentsInChildren<TMP_InputField>();
        Image preview = ui.GetComponentsInChildren<Image>().Last();

        preview.sprite = Sprite.Create(
            initialValue,
            new Rect(0, 0, initialValue.width, initialValue.height),
            new Vector2(0.5f, 0.5f),
            100f
        );

        (Vector2, Action<int, Vector2>)[] textureData = new (Vector2, Action<int, Vector2>)[] { (material.GetTextureScale(nameID), (int nameId, Vector2 lastPartData) => material.SetTextureScale(nameID, lastPartData)), (material.GetTextureOffset(nameID), (int nameId, Vector2 lastPartData) => material.SetTextureOffset(nameID, lastPartData)) };
        texturesDefaultData = textureData;
        texturesLastData = texturesDefaultData.Select(x => x.Item1).ToArray();

        int partIndex = 0;
        for (int i = 0; i < textfields.Length; i += 2)
        {
            TMP_InputField textfieldX = textfields[i];
            TMP_InputField textfieldY = textfields[i + 1];

            Vector2 partData = textureData[partIndex].Item1;
            Action<int, Vector2> defaultAction = texturesDefaultData[partIndex].Item2;
            Vector2 lastPartData = textureData[partIndex].Item1;

            textfieldX.text = partData.x.ToString();
            textfieldY.text = partData.y.ToString();

            textfieldX.onValueChanged.AddListener(delegate
            {
                if (float.TryParse(textfieldX.text.Replace(".", ","), out float parsedValue))
                {
                    lastPartData.x = parsedValue;
                    defaultAction(nameID, lastPartData);
                }
                else
                    textfieldX.text = lastPartData.x.ToString();
            });

            textfieldY.onValueChanged.AddListener(delegate
            {
                if (float.TryParse(textfieldY.text.Replace(".", ","), out float parsedValue))
                {
                    lastPartData.y = parsedValue;
                    defaultAction(nameID, lastPartData);
                }
                else
                    textfieldY.text = lastPartData.y.ToString();
            });

            partIndex++;
        }
    }

    public override void ResetValue()
    {
        material.SetTexture(nameID, defaultValue);
        material.SetTextureScale(nameID, texturesDefaultData[0].Item1);
        material.SetTextureOffset(nameID, texturesDefaultData[1].Item1);

        TMP_InputField[] textfields = UI.GetComponentsInChildren<TMP_InputField>();

        int partIndex = 0;
        for (int i = 0; i < textfields.Length; i += 2)
        {
            textfields[i].text = texturesLastData[partIndex].x.ToString();
            textfields[i + 1].text = texturesLastData[partIndex].y.ToString();

            partIndex++;
        }
    }
}