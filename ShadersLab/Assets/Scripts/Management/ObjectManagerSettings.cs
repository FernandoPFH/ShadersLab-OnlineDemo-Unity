using UnityEngine;

using FernandoPFH_Essentials_Runtime;

[CreateAssetMenu(fileName = "ObjectManagerSettings", menuName = "ScriptableObjects/Management/ObjectManagerSettings")]
public class ObjectManagerSettings : SelfLoadedScriptableObject<ObjectManagerSettings>
{
    public static SerializableDictionary<string, GameObject> PossibleObjects => Instance.possibleObjects;
    [SerializeField] private SerializableDictionary<string, GameObject> possibleObjects;
}