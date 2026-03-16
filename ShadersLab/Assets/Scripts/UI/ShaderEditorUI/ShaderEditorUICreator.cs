using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ShaderEditorUICreatorBase : ScriptableObject
{
    [SerializeField] private GameObject UIprefab;
    protected Material material;

    public virtual GameObject GenerateUIPerProperty(Material material, Shader shader, int propertyIndex)
        => GenerateUI(material, shader, propertyIndex);

    public virtual GameObject GenerateUIPerProperty<T>(string label, Action<T> onChange, T defaultValue)
        => GenerateUI(label, onChange, defaultValue);

    public virtual GameObject GenerateUIPerAttribute(Material material, Shader shader, int propertyIndex, int attributeIndex)
        => GenerateUI(material, shader, propertyIndex);



    private GameObject GenerateUI<T>(string label, Action<T> onChange, T defaultValue)
    {
        GameObject ui = Instantiate(UIprefab);
        ui.GetComponentInChildren<TextMeshProUGUI>().text = $"{label}:";

        return ui;
    }

    private GameObject GenerateUI(Material material, Shader shader, int propertyIndex)
    {
        this.material = material;

        GameObject ui = Instantiate(UIprefab);
        ui.GetComponentInChildren<TextMeshProUGUI>().text = $"{shader.GetPropertyDescription(propertyIndex)}:";

        return ui;
    }
    public abstract void OnSceneExit();
    public abstract void OnAplicationExit();
}

public abstract class ShaderEditorUICreator<T> : ShaderEditorUICreatorBase
{
    protected List<T> shaderUIDataHolders = new();
    public override void OnSceneExit()
        => shaderUIDataHolders.ForEach(x => (x as ShaderUIDataBase).ResetValue());
    public override void OnAplicationExit()
        => shaderUIDataHolders.ForEach(x => (x as ShaderUIDataBase).ResetValue());
}

public abstract class ShaderUIDataBase
{
    public abstract void ResetValue();
}

public abstract class ShaderUIData<T> : ShaderUIDataBase
{
    protected Material material;
    protected int propertyIndex;
    protected int nameID;
    protected T defaultValue;
    protected T lastValue;
    protected GameObject UI;
    protected int attributeIndex;
    public ShaderUIData(Material material, int propertyIndex, T initialValue, GameObject ui)
    {
        this.material = material;
        this.propertyIndex = propertyIndex;
        nameID = material.shader.GetPropertyNameId(propertyIndex);

        EventTrigger.Entry entryPress = new EventTrigger.Entry();
        entryPress.eventID = EventTriggerType.PointerClick;
        entryPress.callback.AddListener((eventData) => { if (eventData is PointerEventData pointerEventData && pointerEventData.button is PointerEventData.InputButton.Middle) ResetValue(); });
        ui.GetComponent<EventTrigger>().triggers.Add(entryPress);

        defaultValue = initialValue;
        lastValue = initialValue;

        UI = ui;
    }

    public ShaderUIData(Material material, int propertyIndex, int attributeIndex, T initialValue, GameObject ui)
    {
        this.material = material;
        this.propertyIndex = propertyIndex;
        this.attributeIndex = attributeIndex;
        nameID = material.shader.GetPropertyNameId(propertyIndex);

        EventTrigger.Entry entryPress = new EventTrigger.Entry();
        entryPress.eventID = EventTriggerType.PointerClick;
        entryPress.callback.AddListener((eventData) => { if (eventData is PointerEventData pointerEventData && pointerEventData.button is PointerEventData.InputButton.Middle) ResetValue(); });
        ui.GetComponent<EventTrigger>().triggers.Add(entryPress);

        defaultValue = initialValue;
        lastValue = initialValue;

        UI = ui;
    }

    public ShaderUIData(T initialValue, GameObject ui)
    {
        // EventTrigger.Entry entryPress = new EventTrigger.Entry();
        // entryPress.eventID = EventTriggerType.PointerClick;
        // entryPress.callback.AddListener((eventData) => { if (eventData is PointerEventData pointerEventData && pointerEventData.button is PointerEventData.InputButton.Middle) ResetValue(); });
        // ui.GetComponent<EventTrigger>().triggers.Add(entryPress);

        defaultValue = initialValue;
        lastValue = initialValue;

        UI = ui;
    }

}
