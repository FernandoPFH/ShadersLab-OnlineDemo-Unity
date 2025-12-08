using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TexturePickerUI : MonoBehaviour
{
    [SerializeField] private Image texturePreview;
    [SerializeField] private GameObject windowPrefab;

    public UnityEvent<Texture2D> onValueChanged = new();

    private static TexturePickerUIWindow texturePickerUIWindowRef;

    public void SetTexture(Texture2D texture)
    {
        texturePreview.sprite = Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100.0f
        );
    }

    public void HandlePress()
    {
        if (!texturePickerUIWindowRef)
            texturePickerUIWindowRef = Instantiate(windowPrefab).GetComponent<TexturePickerUIWindow>();

        texturePickerUIWindowRef.SetupUI(new(Input.mousePosition.x, -Input.mousePosition.y), SceneTransitionManager.CurrentShaderInfos);

        texturePickerUIWindowRef.onValueChanged.AddListener(UpdateTexture);
    }

    private void UpdateTexture(Texture2D texture)
    {
        texturePreview.sprite = Sprite.Create(
            texture,
            new Rect(0.0f, 0.0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f),
            100.0f
        );
        onValueChanged.Invoke(texture);
    }
}
