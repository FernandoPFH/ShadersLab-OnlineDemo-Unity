using UnityEngine;

[CreateAssetMenu(fileName = "ShaderImageTakerSettings", menuName = "ScriptableObjects/Utilities/ShaderImageTakerSettings")]
public class ShaderImageTakerSettings : SelfLoadedScriptableObject<ShaderImageTakerSettings>

{
    public static RenderTexture CameraTexture => Instance.cameraTexture;
    [SerializeField]
    private RenderTexture cameraTexture;

    public static int ImageSize => Instance.imageSize;
    [SerializeField]
    private int imageSize = 1024;

    public static Vector2Int RefScreenSize => Instance.refScreenSize;
    [SerializeField]
    private Vector2Int refScreenSize = new(1920, 1080);


    public static Color ChromaKeyColor => Instance.chromaKeyColor;
    [SerializeField]
    private Color chromaKeyColor = Color.green;

    public static float ChromaKeyTolerance => Instance.chromaKeyTolerance;
    [SerializeField]
    private float chromaKeyTolerance = 0.2f;
}
