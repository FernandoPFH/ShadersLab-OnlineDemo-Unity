using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPickerUIWindow : Singleton<ColorPickerUIWindow>, IDragHandler
{
    [SerializeField] private Image colorPreview;
    [SerializeField] private ColorPickerGradientUI colorPickerGradientUI;
    [SerializeField] private ColorPickerWheelUI colorPickerWheelUI;
    [SerializeField] private Slider colorAlphaSlider;
    [SerializeField] private Image colorAlphaSliderBackground;
    [SerializeField] private TMP_InputField colorAlphaInputField;
    [SerializeField] private Slider colorIntensitySlider;
    [SerializeField] private TMP_InputField colorIntensityInputField;
    [SerializeField] private Transform colorIntensityHolder;

    public Color Color => FromHLSAIToRGB(colorHSLAI);
    public Color ColorWithoutHDR => FromHLSAToRGB(colorHSLAI);
    public UnityEvent<ColorData> onValueChanged = new();

    private HSLAI colorHSLAI;

    public void SetColor(Color color)
    {
        DecomposeHdrColor(color, out Color32 baseLinearColor, out float intensity);
        Color.RGBToHSV(baseLinearColor, out float H, out float S, out float V);
        colorHSLAI = new() { H = H, S = S, L = V, A = color.a, I = intensity };

        colorPickerWheelUI.SetHue(colorHSLAI.H);
        colorPickerGradientUI.SetHue(colorHSLAI.H);
        colorPickerGradientUI.SetSaturationLightness(new(colorHSLAI.S, colorHSLAI.L));

        colorAlphaInputField.text = colorHSLAI.A.ToString();
        colorAlphaSlider.value = colorHSLAI.A;
        colorAlphaSliderBackground.material.SetColor("_Color", ColorWithoutHDR);

        colorIntensityInputField.text = colorHSLAI.I.ToString();
        colorIntensitySlider.value = colorHSLAI.I;

        UpdateColorPreview();
    }

    public void UpdateHue(float hue)
    {
        colorHSLAI.H = hue;

        colorAlphaSliderBackground.material.SetColor("_Color", ColorWithoutHDR);
        colorPickerGradientUI.SetHue(colorHSLAI.H);

        UpdateColorPreview();
    }

    public void UpdateSaturationLightness(Vector2 saturationLightness)
    {
        colorHSLAI.S = saturationLightness.x;
        colorHSLAI.L = saturationLightness.y;

        colorAlphaSliderBackground.material.SetColor("_Color", ColorWithoutHDR);

        UpdateColorPreview();
    }

    public void UpdateAlpha(float alpha)
    {
        colorHSLAI.A = alpha;

        colorAlphaInputField.text = alpha.ToString();

        UpdateColorPreview();
    }

    public void UpdateAlpha(string alpha)
    {
        if (float.TryParse(alpha, out float parsedAlpha))
        {
            colorHSLAI.A = parsedAlpha;

            colorAlphaSlider.value = parsedAlpha;

            UpdateColorPreview();
        }
    }

    public void ResetAlphaIfNecessary(string alpha)
    {
        if (float.TryParse(alpha, out float parsedAlpha))
            UpdateAlpha(alpha);
        else
            colorAlphaInputField.text = colorHSLAI.A.ToString();
    }

    public void UpdateIntensity(float intensity)
    {
        colorHSLAI.I = intensity;

        colorIntensityInputField.text = intensity.ToString();

        UpdateColorPreview();
    }

    public void UpdateIntensity(string intensity)
    {
        if (float.TryParse(intensity, out float parsedintensity))
        {
            colorHSLAI.I = parsedintensity;

            colorIntensitySlider.value = parsedintensity;

            UpdateColorPreview();
        }
    }

    public void ResetIntensityIfNecessary(string intensity)
    {
        if (float.TryParse(intensity, out float parsedIntensity))
            UpdateIntensity(intensity);
        else
            colorIntensityInputField.text = colorHSLAI.I.ToString();
    }

    public void UpdateColorPreview()
    {
        colorPreview.color = ColorWithoutHDR;
        onValueChanged.Invoke(new() { FullColor = Color, ColorWithoutHDR = ColorWithoutHDR });
    }

    private Color FromHLSAIToRGB(HSLAI colorHSLAI)
    {
        float intensityMultiplier = Mathf.Pow(2, colorHSLAI.I);

        Color color = Color.HSVToRGB(colorHSLAI.H, colorHSLAI.S, colorHSLAI.L);
        color.r *= intensityMultiplier;
        color.g *= intensityMultiplier;
        color.b *= intensityMultiplier;
        color.a = colorHSLAI.A;
        return color;
    }

    private Color FromHLSAToRGB(HSLAI colorHSLAI)
    {
        Color color = Color.HSVToRGB(colorHSLAI.H, colorHSLAI.S, colorHSLAI.L);
        color.a = colorHSLAI.A;
        return color;
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

    public void SetupUI(Vector2 screenPosition, bool isHDR, Color color)
    {
        transform.SetParent(FindFirstObjectByType<Canvas>().transform);
        (transform as RectTransform).anchoredPosition = screenPosition;

        SetColor(color);

        ToggleHDR(isHDR);

        gameObject.SetActive(true);
    }

    private void ToggleHDR(bool isHDR)
        => colorIntensityHolder.gameObject.SetActive(isHDR);

    public void CloseWindow()
    {
        gameObject.SetActive(false);
        onValueChanged = new();
    }

    public void OnDrag(PointerEventData eventData)
        => (transform as RectTransform).anchoredPosition += eventData.delta;

    struct HSLAI
    {
        public float H;
        public float S;
        public float L;
        public float A;
        public float I;
    }
}

public struct ColorData
{
    public Color FullColor;
    public Color ColorWithoutHDR;
}