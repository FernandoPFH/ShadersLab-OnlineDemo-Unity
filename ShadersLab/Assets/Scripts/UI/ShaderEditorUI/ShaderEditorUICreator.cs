using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ShaderEditorUICreatorBase : ScriptableObject
{
    [SerializeField] private GameObject UIprefab;
    protected Material material;
    public virtual GameObject GenerateUI(Material material, Shader shader, int propertyIndex)
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
}
