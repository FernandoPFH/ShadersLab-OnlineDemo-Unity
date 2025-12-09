using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObjectSettingsUI : Singleton<ObjectSettingsUI>
{
    [SerializeField] private TMP_Dropdown dropdown;

    private RectTransform rectTransform;
    private float? height;

    private void OnEnable()
        => rectTransform = Instance.transform as RectTransform;

    public static void SteupUI()
    {
        if (!Instance.height.HasValue)
            Instance.height = Instance.rectTransform.sizeDelta.y;

        if (!ObjectManager.IsCurrentlyEnabled)
        {
            Instance.rectTransform.sizeDelta = new(Instance.rectTransform.sizeDelta.x, 0f);
            Instance.gameObject.SetActive(false);
        }
        else
        {
            Instance.rectTransform.sizeDelta = new(Instance.rectTransform.sizeDelta.x, Instance.height.Value);
            Instance.gameObject.SetActive(true);

            List<string> possibleObjectsNames = ObjectManagerSettings.PossibleObjects.Keys.ToList();

            Instance.dropdown.AddOptions(possibleObjectsNames);
            Instance.dropdown.value = possibleObjectsNames.IndexOf(ObjectManager.CurrentObjectName);
        }
    }

    public void UpdateObject(int objectIndex)
    {
        string objectName = ObjectManagerSettings.PossibleObjects.Keys.ToList()[objectIndex];

        ObjectManager.ChangeObject(objectName);
    }
}
