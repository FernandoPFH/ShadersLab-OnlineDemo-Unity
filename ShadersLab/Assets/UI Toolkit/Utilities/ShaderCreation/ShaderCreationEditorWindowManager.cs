using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShaderCreationEditorWindowManager : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    public static Action<string, string, bool> OnSubmit;

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement UXML = m_VisualTreeAsset.Instantiate();
        root.Add(UXML);

        // Populate Type Dropdown
        DropdownField typeField = root.Q<DropdownField>("Type");
        typeField.choices = TipoInfos.Instances.Keys.ToList();
        typeField.value = typeField.choices.First();

        // Get Is Full Screen Toggle
        Toggle toggleFullScreen = root.Q<Toggle>("IsFullScreen");

        // Populate OnClick event
        root.Q<Button>("Submit").clicked += () => Submit(root.Q<TextField>("Name").value, typeField.value, toggleFullScreen.value);
    }

    private void Submit(string shaderName, string typeName, bool isFullscreenEffect)
    {
        OnSubmit.Invoke(shaderName, typeName, isFullscreenEffect);

        Close();
    }

    private void OnDestroy()
        => OnSubmit = null;
}
