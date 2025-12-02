using UnityEngine;

[CreateAssetMenu(fileName = "SearchBarSettings", menuName = "ScriptableObjects/UI/SearchBarSettings")]
public class SearchBarSettings : SelfLoadedScriptableObject<SearchBarSettings>
{
    public static float SpacingBetweenTopBar => Instance.spacingBetweenTopBar;
    [SerializeField]
    private float spacingBetweenTopBar = 16f;
}
