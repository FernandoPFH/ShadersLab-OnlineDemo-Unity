using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShaderEditorUI : Singleton<ShaderEditorUI>
{
    [SerializeField] private Transform scrollViewContentContainer;
    [SerializeField] private GameObject header;

    private float openHeight;
    private float closeHeight;
    private RectTransform rectTransform;


    void Start()
    {
        rectTransform = transform as RectTransform;

        VerticalLayoutGroup verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();

        openHeight = rectTransform.sizeDelta.y;
        closeHeight = header.GetComponent<LayoutElement>().minHeight + verticalLayoutGroup.padding.top + verticalLayoutGroup.padding.bottom;

        rectTransform.sizeDelta = new(rectTransform.sizeDelta.x, closeHeight);
    }

    private bool CheckForUICreatorAndBlockRest(string shaderInfo, Material material, int propertyIndex)
    {
        foreach (string pattern in ShaderEditorUISettings.UICreatorPerType.Keys)
        {
            if (Regex.IsMatch(shaderInfo, pattern))
            {
                ShaderEditorUISettings.UICreatorPerType[pattern].ShaderEditorUICreatorBase.GenerateUIPerProperty(material, material.shader, propertyIndex).transform.SetParent(Instance.scrollViewContentContainer, false);
                return ShaderEditorUISettings.UICreatorPerType[pattern].HasToBlockRest;
            }
        }

        return false;
    }

    private bool CheckForUICreatorAndBlockRest(string shaderInfo, Material material, int propertyIndex, int attributeIndex)
    {
        foreach (string pattern in ShaderEditorUISettings.UICreatorPerType.Keys)
        {
            if (Regex.IsMatch(shaderInfo, pattern))
            {
                ShaderEditorUISettings.UICreatorPerType[pattern].ShaderEditorUICreatorBase.GenerateUIPerAttribute(material, material.shader, propertyIndex, attributeIndex).transform.SetParent(Instance.scrollViewContentContainer, false);
                return ShaderEditorUISettings.UICreatorPerType[pattern].HasToBlockRest;
            }
        }

        return false;
    }

    private static string[] GetActiveFlags(ShaderPropertyFlags flags)
    {
        List<string> result = new();

        foreach (ShaderPropertyFlags value in Enum.GetValues(typeof(ShaderPropertyFlags)))
        {
            if (value == ShaderPropertyFlags.None)
                continue;

            if ((flags & value) != 0)
                result.Add(value.ToString());
        }

        return result.ToArray();
    }

    public static void GenerateUI(Material material)
    {
        for (int i = 0; i < material.shader.GetPropertyCount(); i++)
        {
            bool blockRest = false;
            string[] attributes = material.shader.GetPropertyAttributes(i);
            foreach (string attribute in attributes)
            {
                if (Instance.CheckForUICreatorAndBlockRest(attribute, material, i, Array.IndexOf(attributes, attribute)))
                {
                    blockRest = true;
                    break;
                }
            }

            if (blockRest)
                continue;

            foreach (string flag in GetActiveFlags(material.shader.GetPropertyFlags(i)))
            {
                if (Instance.CheckForUICreatorAndBlockRest(flag, material, i))
                {
                    blockRest = true;
                    break;
                }
            }

            if (blockRest)
                continue;

            Instance.CheckForUICreatorAndBlockRest(material.shader.GetPropertyType(i).ToString(), material, i);
        }
    }

    public static void ClearUI()
    {
        foreach (ShaderEditorUISettings.ShaderUICheck shaderUICheck in ShaderEditorUISettings.UICreatorPerType.Values)
            shaderUICheck.ShaderEditorUICreatorBase.OnSceneExit();
    }

    public void HandleToggle(bool isOn)
        => rectTransform.sizeDelta = isOn ? new(rectTransform.sizeDelta.x, openHeight) : new(rectTransform.sizeDelta.x, closeHeight);

    private void OnApplicationQuit()
    {
        foreach (ShaderEditorUISettings.ShaderUICheck shaderUICheck in ShaderEditorUISettings.UICreatorPerType.Values)
            shaderUICheck.ShaderEditorUICreatorBase.OnAplicationExit();
    }
}
