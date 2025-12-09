using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlaceSettingsUI : Singleton<PlaceSettingsUI>
{
    [SerializeField] private TMP_Dropdown dropdown;

    public static void SteupUI()
    {
        List<string> possiblePlacesNames = PlaceManagerSettings.PossiblePlaces.Keys.ToList();

        Instance.dropdown.AddOptions(possiblePlacesNames);
        Instance.dropdown.value = PlaceManager.IsCurrentlyEnabled ? possiblePlacesNames.IndexOf(PlaceManager.CurrentPlaceName) : 0;
    }

    public void UpdatePlace(int placeIndex)
    {
        string placeName = PlaceManagerSettings.PossiblePlaces.Keys.ToList()[placeIndex];

        PlaceManager.ChangePlace(placeName);
    }
}
