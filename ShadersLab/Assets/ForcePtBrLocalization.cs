using UnityEngine;

public static class ForcePtBrLocalization
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void RunBeforeSceneLoad()
    {
        FernandoPFH_Essentials_Runtime.LocalizationForcer.ForceCulture();
    }
}