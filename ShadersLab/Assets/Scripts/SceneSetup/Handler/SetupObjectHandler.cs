using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SetupObjectHandler", menuName = "ScriptableObjects/Scene Setup/Handler/SetupObjectHandler")]
public class SetupObjectHandler : SceneSetupHandler
{
    [SerializeField] private SerializableDictionary<string, GameObject> defaultObjects;

    private GameObject currentObject;

    public override void OnSceneStart()
    {
        GameObject chosenObject;

        if (SceneTransitionManager.CurrentShaderInfos.TemObjetoInicial)
            chosenObject = defaultObjects[SceneTransitionManager.CurrentShaderInfos.NomeObjetoInicial];
        else
            chosenObject = defaultObjects.Values.First();

        currentObject = Instantiate(chosenObject);
        currentObject.transform.position = Vector3.zero;
    }

    public override void OnSceneExit()
        => Destroy(currentObject);

    public override void OnAplicationExit()
        => Destroy(currentObject);

    public override void SetupNewMaterial(Material material)
        => currentObject.GetComponent<MeshRenderer>().material = material;
}
