using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Shader", menuName = "ScriptableObjects/ShaderInfos")]
public class ShaderInfos : SelfLoadedScriptableObject<ShaderInfos>
{
    public static HashSet<ShaderInfos> Instances { get; private set; } = new();

    protected override void SetInstance()
        => Instances.Add(this);

    public string Nome;
    public TipoInfos Tipo;
    public Sprite MainImage;
    public Material Material;
    public bool TemObjetoInicial;
    [ShowIf("TemObjetoInicial")]
    public string NomeObjetoInicial;
    public List<string> texturesToShow = new();

    public SceneSetupHandler SceneSetupHandler => overrideTypeSceneSetupHandler ? overrideTypeSceneSetupHandler : Tipo.sceneSetupHandler;

    [SerializeField] private SceneSetupHandler overrideTypeSceneSetupHandler;

    public CameraHandler CameraHandler => overrideTypeCameraHandler ? overrideTypeCameraHandler : Tipo.cameraHandler;
    [SerializeField] private CameraHandler overrideTypeCameraHandler;
}