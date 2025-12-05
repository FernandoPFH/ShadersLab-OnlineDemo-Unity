using UnityEngine;

[CreateAssetMenu(fileName = "ShaderEditorUISettings", menuName = "ScriptableObjects/UI/ShaderEditorUI/ShaderEditorUISettings")]
public class ShaderEditorUISettings : SelfLoadedScriptableObject<ShaderEditorUISettings>
{
    public static SerializableDictionary<string, ShaderEditorUICreatorBase> UICreatorPerType => Instance.uiCreatorPerType;
    [SerializeField] private SerializableDictionary<string, ShaderEditorUICreatorBase> uiCreatorPerType;
}
