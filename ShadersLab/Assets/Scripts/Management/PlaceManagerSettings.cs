using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceManagerSettings", menuName = "ScriptableObjects/Management/PlaceManagerSettings")]
public class PlaceManagerSettings : SelfLoadedScriptableObject<PlaceManagerSettings>
{
    public static SerializableDictionary<string, PlaceData> PossiblePlaces => Instance.possiblePlaces;
    [SerializeField] private SerializableDictionary<string, PlaceData> possiblePlaces;

    [Serializable]
    public struct PlaceData
    {
        public GameObject Prefab;
        public Vector3 Position;
        public Quaternion Rotation => Quaternion.Euler(rotation);
        [SerializeField] private Vector3 rotation;
    }
}