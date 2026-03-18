using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "PixelizerShaderInfos", menuName = "ScriptableObjects/Custom/PixelizerShaderInfos")]
public class PixelizerShaderInfos : ShaderInfos
{
    public Material StencilMaterial;

    protected override void GenerateShaderUI()
    {
        (SceneSetupHandler as SetupPixelizerHandler).GenerateUI();

        base.GenerateShaderUI();

        (SceneSetupHandler as SetupPixelizerHandler).GenerateStencilUI(StencilMaterial);
    }

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        (SceneSetupHandler as SetupPixelizerHandler).SetupNewMaterial(Material, StencilMaterial);
    }
}
