using UnityEngine;

[CreateAssetMenu(fileName = "ShaderCreationSettings", menuName = "ScriptableObjects/Utilities/ShaderCreationSettings")]
public class ShaderCreationSettings : SelfLoadedScriptableObject<ShaderCreationSettings>
{
    public static string ShadersFolderPath => Instance.shadersFolderPath;
    [SerializeField]
    private string shadersFolderPath = "Assets/Shaders";
    public static string ShadersResourcesFolderPath => Instance.shadersResourcesFolderPath;
    [SerializeField]
    private string shadersResourcesFolderPath = "Assets/Resources/Shaders";

    [Header("Placeholders/Templates")]
    public static Sprite PlaceholdImage => Instance.placeholdImage;
    [SerializeField]
    private Sprite placeholdImage;

    public static TextAsset ReadmeTemplate => Instance.readmeTemplate;
    [SerializeField]
    private TextAsset readmeTemplate;

    public static TextAsset ShaderTemplate => Instance.shaderTemplate;
    [SerializeField]
    private TextAsset shaderTemplate;

    public static TextAsset ShaderGraphTemplate => Instance.shaderGraphTemplate;
    [SerializeField]
    private TextAsset shaderGraphTemplate;

    public static string TestAreaSceneName => Instance.testAreaSceneName;
    [SerializeField]
    private string testAreaSceneName;

    public static string TestAreaObjectTag => Instance.testAreaObjectTag;
    [SerializeField]
    private string testAreaObjectTag;
}