using UnityEngine;

public abstract class SceneSetupHandler : ScriptableObject
{
    public abstract void OnSceneStart();
    public abstract void OnSceneExit();
    public abstract void OnAplicationExit();
    public abstract void SetupNewMaterial(Material material);
}
