using UnityEngine;

[CreateAssetMenu(fileName = "ItemGridSettings", menuName = "ScriptableObjects/UI/ItemGridSettings")]
public class ItemGridSettings : SelfLoadedScriptableObject<ItemGridSettings>
{
    public static int HorizontalPaddingWhenCompact => Instance.horizontalPaddingWhenCompact;
    [SerializeField]
    private int horizontalPaddingWhenCompact = 40;
}
