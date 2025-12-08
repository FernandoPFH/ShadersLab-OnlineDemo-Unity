using UnityEngine;

[CreateAssetMenu(fileName = "LabelDraggerSettings", menuName = "ScriptableObjects/UI/GenericElements/LabelDraggerSettings")]
public class LabelDraggerSettings : SelfLoadedScriptableObject<LabelDraggerSettings>
{
    public static float DragMultiplier => Instance.dragMultiplier;
    [SerializeField] private float dragMultiplier = 0.01f;
    public static Texture2D DragCursorTexture => Instance.dragCursorTexture;
    [SerializeField] private Texture2D dragCursorTexture = null;
}
