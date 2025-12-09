using UnityEngine;

[CreateAssetMenu(fileName = "SetupObjectHandler", menuName = "ScriptableObjects/Scene Setup/Handler/SetupObjectHandler")]
public class SetupObjectHandler : SceneSetupHandler
{
    public override void OnSceneStart()
        => ObjectManager.CreateObject();

    public override void OnSceneExit()
        => ObjectManager.TryDestroyObject();

    public override void OnAplicationExit()
        => ObjectManager.TryDestroyObject();

    public override void SetupNewMaterial(Material material)
        => ObjectManager.SetupMaterial(material);
}
