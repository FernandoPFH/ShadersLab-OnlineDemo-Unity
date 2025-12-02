using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationBackgroundObjectsSettings", menuName = "ScriptableObjects/UI 3D/AnimationBackgroundObjectsSettings")]
public class AnimationBackgroundObjectsSettings : SelfLoadedScriptableObject<AnimationBackgroundObjectsSettings>
{
    public static float InitialDelay => Instance.initialDelay;
    [SerializeField]
    private float initialDelay = 1f;

    public static float BetweenDelay => Instance.betweenDelay;
    [SerializeField]
    private float betweenDelay = 2f;

    public static SerializableDictionary<string, MaterialWithAnimation> AnimationCurvePerColor => Instance.animationCurvePerColor;
    [SerializeField]
    private SerializableDictionary<string, MaterialWithAnimation> animationCurvePerColor;

    [Serializable]
    public struct MaterialWithAnimation
    {
        public Material material;
        public AnimationCurve animationCurve;
    }

    public static int ShaderProperty => Shader.PropertyToID(Instance.shaderProperty);
    [SerializeField]
    private string shaderProperty = "_CutoffHeight";
}
