using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Type", menuName = "ScriptableObjects/TypeInfos", order = 1)]
public class TipoInfos : SelfLoadedScriptableObject<TipoInfos>
{
    public static Dictionary<string, TipoInfos> Instances { get; private set; } = new();

    protected override void SetInstance()
        => Instances[Nome] = this;

    public string Nome;
    public Sprite Icone;
    public bool IsShaderGraph = false;
    public SceneSetupHandler sceneSetupHandler;
    public CameraHandler cameraHandler;
}
