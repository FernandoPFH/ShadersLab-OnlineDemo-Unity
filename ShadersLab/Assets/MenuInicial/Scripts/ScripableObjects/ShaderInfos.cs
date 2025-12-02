using System.Collections.Generic;
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
}