using UnityEngine;


public abstract class SelfLoadedScriptableObject<T> : ScriptableObject where T : SelfLoadedScriptableObject<T>
{
    protected static T Instance { get; private set; }

    protected virtual void OnEnable()
        => Instance = this as T;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this as T;
    }
}