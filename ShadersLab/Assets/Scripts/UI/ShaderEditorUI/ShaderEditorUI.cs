using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ShaderEditorUI : Singleton<ShaderEditorUI>
{
    [SerializeField] private Transform scrollViewContentContainer;
    [SerializeField] private EventTrigger eventTrigger;
    public static bool IsUIBeingHover => Instance.isUIBeingHover;
    private bool isUIBeingHover;

    void Start()
    {
        EventTrigger.Entry entryHoverEnter = new EventTrigger.Entry();
        entryHoverEnter.eventID = EventTriggerType.PointerEnter;
        entryHoverEnter.callback.AddListener((eventData) => { isUIBeingHover = true; });
        eventTrigger.triggers.Add(entryHoverEnter);

        EventTrigger.Entry entryHoverExit = new EventTrigger.Entry();
        entryHoverExit.eventID = EventTriggerType.PointerExit;
        entryHoverExit.callback.AddListener((eventData) => { isUIBeingHover = false; });
        eventTrigger.triggers.Add(entryHoverExit);
    }

    void OnValidate()
    {
        if (eventTrigger) eventTrigger = GetComponent<EventTrigger>();
    }

    public static void GenerateUI(Material material)
    {
        for (int i = 0; i < material.shader.GetPropertyCount(); i++)
        {
            bool foundAttribute = false;
            foreach (string attribute in material.shader.GetPropertyAttributes(i))
                if (ShaderEditorUISettings.UICreatorPerType.TryGetValue(attribute.Split("(")[0], out ShaderEditorUICreatorBase shaderEditorUICreatorForAttribute))
                {
                    shaderEditorUICreatorForAttribute.GenerateUI(material, material.shader, i).transform.SetParent(Instance.scrollViewContentContainer, false);
                    foundAttribute = true;
                    break;
                }

            if (foundAttribute)
                continue;

            if (ShaderEditorUISettings.UICreatorPerType.TryGetValue(material.shader.GetPropertyType(i).ToString(), out ShaderEditorUICreatorBase shaderEditorUICreatorForProperty))
                shaderEditorUICreatorForProperty.GenerateUI(material, material.shader, i).transform.SetParent(Instance.scrollViewContentContainer, false);
        }
    }

    public static void ClearUI()
    {
        foreach (ShaderEditorUICreatorBase shaderEditorUICreator in ShaderEditorUISettings.UICreatorPerType.Values)
            shaderEditorUICreator.OnSceneExit();
    }

    private void OnApplicationQuit()
    {
        foreach (ShaderEditorUICreatorBase shaderEditorUICreator in ShaderEditorUISettings.UICreatorPerType.Values)
            shaderEditorUICreator.OnAplicationExit();
    }
}
