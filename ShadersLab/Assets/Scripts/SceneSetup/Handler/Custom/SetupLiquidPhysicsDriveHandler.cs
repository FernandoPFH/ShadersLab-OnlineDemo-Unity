using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SetupLiquidPhysicsDriveHandler", menuName = "ScriptableObjects/Scene Setup/Handler/Custom/SetupLiquidPhysicsDriveHandler")]
public class SetupLiquidPhysicsDriveHandler : SceneSetupHandler
{
    [SerializeField] private GameObject liquidWobblePrefab;

    private GameObject liquidWobbleInstance;

    public override void OnSceneStart()
    {
        ObjectManager.CreateObject();
        liquidWobbleInstance = Instantiate(liquidWobblePrefab, ObjectManager.CurrentObject.transform);
    }

    public override void OnSceneExit()
    {
        Destroy(liquidWobbleInstance);
        ObjectManager.TryDestroyObject();
    }

    public override void OnAplicationExit()
    {
        Destroy(liquidWobbleInstance);
        ObjectManager.TryDestroyObject();
    }

    public override void SetupNewMaterial(Material material)
        => ObjectManager.SetupMaterial(material);
}
