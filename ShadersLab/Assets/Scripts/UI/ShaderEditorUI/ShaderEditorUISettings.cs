using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShaderEditorUISettings", menuName = "ScriptableObjects/UI/ShaderEditorUI/ShaderEditorUISettings")]
public class ShaderEditorUISettings : SelfLoadedScriptableObject<ShaderEditorUISettings>
{
    public static SerializableDictionary<string, ShaderUICheck> UICreatorPerType => Instance.uiCreatorPerType;
    [SerializeField] private SerializableDictionary<string, ShaderUICheck> uiCreatorPerType;

    [Serializable]
    public struct ShaderUICheck
    {
        public ShaderEditorUICreatorBase ShaderEditorUICreatorBase;
        public bool HasToBlockRest;
    }
}
