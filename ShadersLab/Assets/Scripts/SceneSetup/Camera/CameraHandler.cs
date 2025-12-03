using UnityEngine;

public abstract class CameraHandler : ScriptableObject
{
    public abstract void OnSceneStart();
    public abstract void OnSceneExit();
    public abstract void OnAplicationExit();
    public abstract void OnUpdate();
}
