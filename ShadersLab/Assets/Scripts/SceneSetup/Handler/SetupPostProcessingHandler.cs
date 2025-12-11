using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "SetupPostProcessingHandler", menuName = "ScriptableObjects/Scene Setup/Handler/SetupPostProcessingHandler")]
public class SetupPostProcessingHandler : SceneSetupHandler
{
    [SerializeField] private UniversalRendererData universalRendererData;
    private FullScreenPassRendererFeature fullScreenPass;

    public override void OnSceneStart()
    {
        foreach (ScriptableRendererFeature feature in (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).rendererDataList[0].rendererFeatures)
            if (feature is FullScreenPassRendererFeature fullScreenPass)
                this.fullScreenPass = fullScreenPass;
    }

    public override void OnSceneExit()
        => fullScreenPass.SetActive(false);

    public override void OnAplicationExit()
        => fullScreenPass.SetActive(false);

    public override void SetupNewMaterial(Material material)
    {
        fullScreenPass.passMaterial = material;

        fullScreenPass.SetActive(true);
    }
}
