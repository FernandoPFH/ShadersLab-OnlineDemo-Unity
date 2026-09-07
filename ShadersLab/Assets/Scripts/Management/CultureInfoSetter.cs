using UnityEngine;
using System.Globalization;
using System.Threading;

public static class CultureInfoSetter
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeCulture()
    {
        // CultureInfo invariant = CultureInfo.InvariantCulture;
        
        CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("pt-BR");
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        Debug.Log("Cultura Global reforçada para InvariantCulture.");
    }
}