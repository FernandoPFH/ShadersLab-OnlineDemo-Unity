using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

[CreateAssetMenu(fileName = "SetupPixelizerHandler", menuName = "ScriptableObjects/Scene Setup/Handler/Custom/SetupPixelizerHandler")]
public class SetupPixelizerHandler : SetupPostProcessingHandler
{
    [SerializeField] private string stencilID;
    private RenderObjects renderObjects;

    protected override void ProcessRendererFeature(ScriptableRendererFeature feature)
    {
        base.ProcessRendererFeature(feature);

        if (feature is RenderObjects renderObjects)
            this.renderObjects = renderObjects;
    }

    public override void OnSceneStart()
    {
        base.OnSceneStart();

        ObjectManager.CreateObject();
    }

    public override void OnSceneExit()
    {
        base.OnSceneExit();

        renderObjects.SetActive(false);
        ObjectManager.TryDestroyObject();
    }

    public override void OnAplicationExit()
    {
        base.OnAplicationExit();

        renderObjects.SetActive(false);
        ObjectManager.TryDestroyObject();
    }

    public void SetupNewMaterial(Material postProcessingMaterial, Material stencilMaterial)
    {
        base.SetupNewMaterial(postProcessingMaterial);
        renderObjects.settings.overrideMaterial = postProcessingMaterial;

        renderObjects.settings.stencilSettings.overrideStencilState = true;
        renderObjects.settings.stencilSettings.stencilReference = stencilMaterial.GetInt(stencilID);

        renderObjects.SetActive(true);
        ObjectManager.SetupMaterial(stencilMaterial);
    }

    public void GenerateUI()
    {
        ShaderEditorUI.GenerateUI("Per Object?", "Toggle", (bool isToggled) =>
        {
            fullScreenPass.SetActive(!isToggled);
        }, false);
    }
}
