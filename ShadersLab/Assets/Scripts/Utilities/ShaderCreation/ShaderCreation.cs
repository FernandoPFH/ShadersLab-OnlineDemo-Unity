using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class ShaderCreation
{
    #region Shader Creation

    [MenuItem("Assets/Create/ShaderLab/Create Shader")]
    private static void OpenShaderCreationEditorWindow()
    {
        ShaderCreationEditorWindowManager.OnSubmit += HandleShaderCreationSubmit;

        ShaderCreationEditorWindowManager wnd = EditorWindow.GetWindow<ShaderCreationEditorWindowManager>();
        wnd.titleContent = new GUIContent("New Shader");
    }

    private static void HandleShaderCreationSubmit(string shaderName, string typeName, bool isFullscreenEffect)
    {
        CreateShaderTemplate(shaderName, typeName, isFullscreenEffect);

        ShaderCreationEditorWindowManager.OnSubmit -= CreateShaderTemplate;
    }

    private static void CreateShaderTemplate(string shaderName, string typeName, bool isFullscreenEffect)
    {
        shaderName = shaderName.Replace(" ", "");
        string shaderNameWithSpaces = Regex.Replace(shaderName, "([A-Z])", " $1").Trim();
        TipoInfos type = TipoInfos.Instances[typeName];

        string shaderRootPath = Path.Combine(ShaderCreationSettings.ShadersFolderPath, shaderName);
        string shaderResourcesRootPath = Path.Combine(ShaderCreationSettings.ShadersResourcesFolderPath, shaderName);

        if (Directory.Exists(shaderResourcesRootPath))
        {
            Debug.LogError("Shader Creation: Shader already exists!");
            return;
        }

        // Create Shader Root directory
        Directory.CreateDirectory(shaderRootPath);
        Directory.CreateDirectory(shaderResourcesRootPath);

        // Create Extras directory and populate it with placehold image
        Directory.CreateDirectory(Path.Combine(shaderResourcesRootPath, "Extras"));
        string mainImagePath = Path.Combine(shaderResourcesRootPath, "Extras", "main.png");
        CreateSprite(ShaderCreationSettings.PlaceholdImage.texture, mainImagePath);

        // Create Shader directory and populate it with an shader and material file
        Material materialCreated = CreateShaderMaterialFiles(shaderName, shaderNameWithSpaces, shaderRootPath, type);

        // Populate Shader Root directory
        File.WriteAllText(Path.Combine(shaderResourcesRootPath, "README.md"), ShaderCreationSettings.ReadmeTemplate.text.PopulateTemplate(new()
        {
            { "Name", shaderNameWithSpaces},
            { "Type", typeName},
            { "TypePath", Path.GetRelativePath(shaderResourcesRootPath,AssetDatabase.GetAssetPath(type.Icone))},
        }));

        ShaderInfos asset = ScriptableObject.CreateInstance<ShaderInfos>();
        asset.Nome = shaderNameWithSpaces;
        asset.Tipo = type;
        asset.MainImage = AssetDatabase.LoadAssetAtPath<Sprite>(mainImagePath);
        AssetDatabase.CreateAsset(asset, Path.Combine(shaderResourcesRootPath, $"{shaderName}.asset"));

        // Update Unity Assets
        AssetDatabase.Refresh();

        OpenTestAreaScene(materialCreated, isFullscreenEffect);
    }

    private static Material CreateShaderMaterialFiles(string shaderName, string shaderNameWithSpaces, string shaderRootPath, TipoInfos type)
    {
        string shaderFilePath = Path.Combine(shaderRootPath, shaderName + (type.IsShaderGraph ? ".shadergraph" : $".shader"));

        Directory.CreateDirectory(shaderRootPath);
        File.WriteAllText(
            shaderFilePath,
            (type.IsShaderGraph ? ShaderCreationSettings.ShaderGraphTemplate.text : ShaderCreationSettings.ShaderTemplate.text).PopulateTemplate(new()
        {
            { "Name", shaderNameWithSpaces},
        }));

        AssetDatabase.ImportAsset(shaderFilePath, ImportAssetOptions.ForceSynchronousImport);
        Shader shaderAsset = AssetDatabase.LoadAssetAtPath<Shader>(shaderFilePath);
        Material material = new Material(shaderAsset);
        AssetDatabase.CreateAsset(material, Path.Combine(shaderRootPath, $"{shaderName}.mat"));

        return material;
    }

    private static void OpenTestAreaScene(Material material, bool isFullscreenEffect)
    {
        UnityEngine.SceneManagement.Scene currentScene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(currentScene);

        if (currentScene.name != ShaderCreationSettings.TestAreaSceneName)
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/{ShaderCreationSettings.TestAreaSceneName}.unity");
        }

        if (!isFullscreenEffect)
        {
            currentScene = EditorSceneManager.GetActiveScene();

            foreach (GameObject rootGameObject in currentScene.GetRootGameObjects())
                if (rootGameObject.tag == ShaderCreationSettings.TestAreaObjectTag)
                    rootGameObject.GetComponent<MeshRenderer>().material = material;

            EditorSceneManager.SaveScene(currentScene);
        }
        else
        {
            foreach (ScriptableRendererFeature feature in (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).rendererDataList[0].rendererFeatures)
                if (feature is FullScreenPassRendererFeature fullScreenPass)
                    fullScreenPass.passMaterial = material;
        }
    }

    #endregion

    #region Shader Maintenance

    [MenuItem("Assets/ShaderLab/Create Missing Files")]
    private static void CreateMissingFiles()
    {
        List<string> shadersDirectories = Directory.GetDirectories(ShaderCreationSettings.ShadersFolderPath).ToList();

        foreach (string shaderDirectory in shadersDirectories)
        {
            string shaderName = shaderDirectory.Split(Path.DirectorySeparatorChar).Last();
            string shaderNameWithSpaces = Regex.Replace(shaderName, "([A-Z])", " $1").Trim();
            TipoInfos type = TipoInfos.Instances.First().Value;
            string shaderResourcesDirectory = Path.Combine(ShaderCreationSettings.ShadersResourcesFolderPath, shaderName);

            if (!Directory.Exists(shaderResourcesDirectory))
                Directory.CreateDirectory(shaderResourcesDirectory);

            if (!Directory.Exists(Path.Combine(shaderResourcesDirectory, "Extras")))
            {
                Directory.CreateDirectory(Path.Combine(shaderResourcesDirectory, "Extras"));

                Material materialAsset = AssetDatabase.LoadAssetAtPath<Material>(Directory.GetFiles(shaderDirectory, "*.mat").First());
                Texture2D preview = AssetPreview.GetAssetPreview(materialAsset);

                int counter = 0;
                while (preview == null && counter < 100000)
                {
                    preview = AssetPreview.GetAssetPreview(materialAsset);
                    counter++;
                }

                CreateSprite(preview, Path.Combine(shaderResourcesDirectory, "Extras", "main.png"));
            }

            if (!File.Exists(Path.Combine(shaderResourcesDirectory, "README.md")))
            {
                File.WriteAllText(Path.Combine(shaderResourcesDirectory, "README.md"), ShaderCreationSettings.ReadmeTemplate.text.PopulateTemplate(new()
                {
                    { "Name", shaderNameWithSpaces},
                    { "Type", type.Nome},
                    { "TypePath", Path.GetRelativePath(shaderResourcesDirectory,AssetDatabase.GetAssetPath(type.Icone))},
                }));
            }

            if (!File.Exists(Path.Combine(shaderResourcesDirectory, $"{shaderName}.asset")))
            {
                ShaderInfos asset = ScriptableObject.CreateInstance<ShaderInfos>();
                asset.Nome = shaderNameWithSpaces;
                asset.Tipo = type;
                asset.MainImage = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(shaderResourcesDirectory, "Extras", "main.png"));
                asset.Material = AssetDatabase.LoadAssetAtPath<Material>(Directory.GetFiles(shaderDirectory, "*.mat").First());
                AssetDatabase.CreateAsset(asset, Path.Combine(shaderResourcesDirectory, $"{shaderName}.asset"));
            }
        }

        // Update Unity Assets
        AssetDatabase.Refresh();
    }

    private static void CreateSprite(Texture2D image, string path)
    {
        Texture2D clonedImage = new Texture2D(image.width, image.height, image.format, image.mipmapCount > 1);

        Graphics.CopyTexture(image, clonedImage);

        File.WriteAllBytes(path, clonedImage.EncodeToPNG());

        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spriteImportMode = SpriteImportMode.Single;
        AssetDatabase.ImportAsset(path);
    }

    #endregion
}
