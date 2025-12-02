using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

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

    private static void HandleShaderCreationSubmit(string shaderName, string typeName)
    {
        CreateShaderTemplate(shaderName, typeName);

        ShaderCreationEditorWindowManager.OnSubmit -= CreateShaderTemplate;
    }

    private static void CreateShaderTemplate(string shaderName, string typeName)
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
        File.Copy(AssetDatabase.GetAssetPath(ShaderCreationSettings.PlaceholdImage), Path.Combine(shaderResourcesRootPath, "Extras", "main.png"));

        // Create Shader directory and populate it with an shader and material file
        CreateShaderMaterialFiles(shaderName, shaderNameWithSpaces, shaderRootPath, type);

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
        asset.MainImage = ShaderCreationSettings.PlaceholdImage;
        AssetDatabase.CreateAsset(asset, Path.Combine(shaderResourcesRootPath, $"{shaderName}.asset"));

        // Update Unity Assets
        AssetDatabase.Refresh();
    }

    private static void CreateShaderMaterialFiles(string shaderName, string shaderNameWithSpaces, string shaderRootPath, TipoInfos type)
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
                File.Copy(AssetDatabase.GetAssetPath(ShaderCreationSettings.PlaceholdImage), Path.Combine(shaderResourcesDirectory, "Extras", "main.png"));
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
                asset.MainImage = ShaderCreationSettings.PlaceholdImage;
                AssetDatabase.CreateAsset(asset, Path.Combine(shaderResourcesDirectory, $"{shaderName}.asset"));
            }
        }

        // Update Unity Assets
        AssetDatabase.Refresh();
    }

    #endregion
}
