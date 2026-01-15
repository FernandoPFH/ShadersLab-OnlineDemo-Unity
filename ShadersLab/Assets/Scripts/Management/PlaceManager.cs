using System.Linq;
using UnityEngine;

public class PlaceManager : Singleton<PlaceManager>
{
    public static bool IsCurrentlyEnabled => CurrentPlace;
    public static GameObject CurrentPlace;
    public static string CurrentPlaceName;

    public static GameObject CreatePlace()
        => Instance.CreatePlace(SceneTransitionManager.CurrentShaderInfos.TemLocalInicial ? SceneTransitionManager.CurrentShaderInfos.NomeLocalInicial : PlaceManagerSettings.PossiblePlaces.Keys.First());

    public GameObject CreatePlace(string placeName)
    {
        PlaceManagerSettings.PlaceData place = PlaceManagerSettings.PossiblePlaces[placeName];
        CurrentPlaceName = placeName;

        CurrentPlace = Instantiate(place.Prefab);
        CurrentPlace.transform.SetPositionAndRotation(place.Position, place.Rotation);

        return CurrentPlace;
    }

    public static void TryDestroyPlace()
    {
        if (CurrentPlace)
        {
            Destroy(CurrentPlace);
            CurrentPlace = null;
        }
    }

    internal static void ChangePlace(string placeName)
    {
        Destroy(CurrentPlace);

        Instance.CreatePlace(placeName);
    }
}
