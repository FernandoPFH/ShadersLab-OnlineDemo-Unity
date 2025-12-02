using UnityEngine;

public abstract class SelfLoadedScriptableObject<T> : ScriptableObject where T : SelfLoadedScriptableObject<T>
{
    protected static T Instance { get; private set; }

    protected virtual void OnEnable()
        => SetInstance();

    protected virtual void Awake()
        => SetInstance();

    protected virtual void SetInstance()
    {
        if (Instance == null)
            Instance = this as T;
    }
}