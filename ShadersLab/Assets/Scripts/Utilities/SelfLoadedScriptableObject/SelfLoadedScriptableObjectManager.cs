using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
class SelfLoadedScriptableObjectManager
{
    static SelfLoadedScriptableObjectManager()
    {
        foreach (string soGUID in AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/Resources" }))
            AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(soGUID));
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadInRuntime()
        => Resources.LoadAll<ScriptableObject>("");
}