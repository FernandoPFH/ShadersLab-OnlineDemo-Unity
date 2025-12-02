using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T Instance { get; private set; }

    protected virtual void Awake()
        => Instance = this as T;
}