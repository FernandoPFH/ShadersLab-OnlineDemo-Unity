using UnityEngine;

[CreateAssetMenu(fileName = "FilterBarSettings", menuName = "ScriptableObjects/UI/FilterBarSettings")]
public class FilterBarSettings : SelfLoadedScriptableObject<FilterBarSettings>
{
    public static float SpacingBetweenSearchBar => Instance.spacingBetweenSearchBar;
    [SerializeField]
    private float spacingBetweenSearchBar = 18f;
}
