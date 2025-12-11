using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
class SelfLoaderScriptableObject
{
    static SelfLoaderScriptableObject()
    {
        foreach (string soGUID in AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/Resources" }))
            AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(soGUID));
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadInRuntime()
        => Resources.LoadAll<ScriptableObject>("");
}
#endif

public class SelfLoadedScriptableObject : MonoBehaviour
{
    void Awake()
        => Resources.LoadAll<ScriptableObject>("");
}