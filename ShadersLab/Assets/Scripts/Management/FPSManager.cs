using UnityEngine;

public class FPSManager : Singleton<FPSManager>
{
    [SerializeField] private int targetFPS = 60;

    protected override void Awake()
    {
        base.Awake();

        // 1. Disable VSync (Required, otherwise targetFrameRate is ignored)
        QualitySettings.vSyncCount = 0;

        // 2. Set the target frame rate
        Application.targetFrameRate = targetFPS;
    }
}
