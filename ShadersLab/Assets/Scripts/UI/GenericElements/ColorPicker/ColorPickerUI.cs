using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    [SerializeField] private Image colorPreview;
    [SerializeField] private GameObject hdrLabel;
    [SerializeField] private GameObject windowPrefab;

    public UnityEvent<ColorData> onValueChanged = new();

    private Color color = Color.red;
    private bool isHDR = true;

    private static ColorPickerUIWindow colorPickerUIWindowRef;

    public void SetColor(Color color)
    {
        DecomposeHdrColor(color, out Color32 baseLinearColor, out float _);
        this.color = color;
        colorPreview.color = baseLinearColor;
    }

    public void HideHDRLabel()
    {
        isHDR = false;
        hdrLabel.SetActive(false);
    }

    public void HandlePress()
    {
        if (!colorPickerUIWindowRef)
            colorPickerUIWindowRef = Instantiate(windowPrefab).GetComponent<ColorPickerUIWindow>();

        colorPickerUIWindowRef.SetupUI(new(Input.mousePosition.x, -Input.mousePosition.y), isHDR, color);

        colorPickerUIWindowRef.onValueChanged.AddListener(UpdateColor);
    }

    private void UpdateColor(ColorData colorData)
    {
        colorPreview.color = colorData.ColorWithoutHDR;
        color = colorData.FullColor;
        onValueChanged.Invoke(colorData);
    }

    private const byte k_MaxByteForOverexposedColor = 191;

    private void DecomposeHdrColor(Color linearColorHdr, out Color32 baseLinearColor, out float exposure)
    {
        baseLinearColor = linearColorHdr;
        var maxColorComponent = linearColorHdr.maxColorComponent;
        // replicate Photoshops's decomposition behaviour
        if (maxColorComponent == 0f || maxColorComponent <= 1f && maxColorComponent >= 1 / 255f)
        {
            exposure = 0f;

            baseLinearColor.r = (byte)Mathf.RoundToInt(linearColorHdr.r * 255f);
            baseLinearColor.g = (byte)Mathf.RoundToInt(linearColorHdr.g * 255f);
            baseLinearColor.b = (byte)Mathf.RoundToInt(linearColorHdr.b * 255f);
        }
        else
        {
            // calibrate exposure to the max float color component
            var scaleFactor = k_MaxByteForOverexposedColor / maxColorComponent;
            exposure = Mathf.Log(255f / scaleFactor) / Mathf.Log(2f);

            // maintain maximal integrity of byte values to prevent off-by-one errors when scaling up a color one component at a time
            baseLinearColor.r = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.r));
            baseLinearColor.g = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.g));
            baseLinearColor.b = Math.Min(k_MaxByteForOverexposedColor, (byte)Mathf.CeilToInt(scaleFactor * linearColorHdr.b));
        }
    }
}
