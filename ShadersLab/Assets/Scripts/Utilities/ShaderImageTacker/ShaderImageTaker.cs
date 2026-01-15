using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

public static class ShaderImageTaker
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/ShaderLab/Take Image")]
    private static void TakeImage(MenuCommand command)
    {
        Texture2D texture = GetScreenTextureCroped();

        RemoveChromaKeyPixels(texture);

        MethodInfo getActiveFolderPath = typeof(ProjectWindowUtil).GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);

        object obj = getActiveFolderPath.Invoke(null, new object[0]);
        string pathToCurrentFolder = obj.ToString();

        CreateSprite(texture, Path.Combine(pathToCurrentFolder, "main.png"));
    }

    private static Texture2D GetScreenTextureCroped()
    {
        int scaleFactor = Mathf.FloorToInt(Screen.height / ShaderImageTakerSettings.RefScreenSize.y);

        Vector2Int borderSize = new((ShaderImageTakerSettings.RefScreenSize.x - ShaderImageTakerSettings.ImageSize) / 2, (ShaderImageTakerSettings.RefScreenSize.y - ShaderImageTakerSettings.ImageSize) / 2);

        borderSize *= scaleFactor;

        RenderTexture.active = ShaderImageTakerSettings.CameraTexture;

        Texture2D texture = new(ShaderImageTakerSettings.ImageSize * scaleFactor, ShaderImageTakerSettings.ImageSize * scaleFactor);
        texture.ReadPixels(new Rect(borderSize.x, borderSize.y, ShaderImageTakerSettings.ImageSize * scaleFactor, ShaderImageTakerSettings.ImageSize * scaleFactor), 0, 0);
        texture.Apply();

        return texture;
    }

    private static void RemoveChromaKeyPixels(Texture2D texture)
    {
        Color[] pixels = texture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
            if (ColorCloseTo(pixels[i], ShaderImageTakerSettings.ChromaKeyColor, ShaderImageTakerSettings.ChromaKeyTolerance))
                pixels[i] = Color.clear; // Set the pixel to transparent

        texture.SetPixels(pixels);
        texture.Apply();
    }

    private static bool ColorCloseTo(Color pixel, Color target, float tolerance)
        => Mathf.Abs(pixel.r - target.r) <= tolerance &&
               Mathf.Abs(pixel.g - target.g) <= tolerance &&
               Mathf.Abs(pixel.b - target.b) <= tolerance;

    private static void CreateSprite(Texture2D image, string path)
    {
        Texture2D clonedImage = new Texture2D(image.width, image.height, image.format, image.mipmapCount > 1);

        Graphics.CopyTexture(image, clonedImage);

        File.WriteAllBytes(path, clonedImage.EncodeToPNG());

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
        TextureImporterPlatformSettings defaultTextureImporterPlatformSettings = textureImporter.GetDefaultPlatformTextureSettings();
        defaultTextureImporterPlatformSettings.format = TextureImporterFormat.RGBAFloat;
        textureImporter.SetPlatformTextureSettings(defaultTextureImporterPlatformSettings);
        AssetDatabase.ImportAsset(path);
    }
#endif
}
