using System;
using System.Linq;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public static bool IsCurrentlyEnabled => CurrentObject;
    public static GameObject CurrentObject;
    public static string CurrentObjectName;
    private Material currentMaterial;

    public static GameObject CreateObject()
        => Instance.CreateObject(SceneTransitionManager.CurrentShaderInfos.TemObjetoInicial ? SceneTransitionManager.CurrentShaderInfos.NomeObjetoInicial : ObjectManagerSettings.PossibleObjects.Keys.First());

    public GameObject CreateObject(string objectName)
    {
        GameObject chosenPrefab = ObjectManagerSettings.PossibleObjects[objectName];
        CurrentObjectName = objectName;

        CurrentObject = Instantiate(chosenPrefab);
        CurrentObject.transform.position = Vector3.zero;

        return CurrentObject;
    }

    public static void TryDestroyObject()
    {
        if (CurrentObject)
        {
            Destroy(CurrentObject);
            CurrentObject = null;
            CurrentObjectName = string.Empty;
        }
    }

    public static void SetupMaterial(Material material)
    {
        Instance.currentMaterial = material;
        CurrentObject.GetComponent<MeshRenderer>().material = material;
    }


    internal static void ChangeObject(string objectName)
    {
        Destroy(CurrentObject);

        Instance.CreateObject(objectName);

        SetupMaterial(Instance.currentMaterial);
    }
}
