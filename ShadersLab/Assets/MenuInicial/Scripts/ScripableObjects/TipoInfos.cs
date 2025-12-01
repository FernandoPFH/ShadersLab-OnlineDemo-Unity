using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Type", menuName = "ScriptableObjects/TypeInfos", order = 1)]
public class TipoInfos : SelfLoadedScriptableObject<TipoInfos>
{
    public static Dictionary<string, TipoInfos> Types => types;
    private static Dictionary<string, TipoInfos> types = new();

    protected override void OnEnable()
    {
        base.OnEnable();
        types[Nome] = this;
    }

    protected override void Awake()
    {
        base.Awake();
        types[Nome] = this;
    }

    public string Nome;
    public Sprite Icone;
    public bool IsShaderGraph = false;
}
