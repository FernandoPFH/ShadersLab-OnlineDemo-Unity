using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TexturePickerSettings", menuName = "ScriptableObjects/UI/GenericElements/TexturePickerSettings")]
public class TexturePickerSettings : SelfLoadedScriptableObject<TexturePickerSettings>
{
    public static SerializableDictionary<string, List<Texture2D>> ListsOfTextures => Instance.listsOfTextures;
    [SerializeField] private SerializableDictionary<string, List<Texture2D>> listsOfTextures;
}
