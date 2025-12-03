using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    private string shaderDisplayScene = "Assets/Scenes/ShaderDisplay.unity";

    public static ShaderInfos CurrentShaderInfos;

    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(transform.parent);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CurrentShaderInfos)
            CloseShaderDisplayScene();

        if (CurrentShaderInfos)
            CurrentShaderInfos.CameraHandler.OnUpdate();
    }

    public static void OpenShaderDisplayScene(ShaderInfos shaderInfos)
    {
        CurrentShaderInfos = shaderInfos;

        TelaDeEscolha.Instance.transform.parent.gameObject.SetActive(false);
        AnimationBackgroundObjects.Instance.gameObject.SetActive(false);
        Instance.mainCamera = Camera.main;
        Instance.mainCamera.gameObject.SetActive(false);

        var op = SceneManager.LoadSceneAsync(Instance.shaderDisplayScene, LoadSceneMode.Additive);
        op.completed += Instance.SetupScene;
    }

    public static void CloseShaderDisplayScene()
    {
        var op = SceneManager.UnloadSceneAsync(Instance.shaderDisplayScene);
        op.completed += Instance.ResetScene;
    }

    private void SetupScene(AsyncOperation _)
    {
        CurrentShaderInfos.SceneSetupHandler.OnSceneStart();
        CurrentShaderInfos.CameraHandler.OnSceneStart();

        CurrentShaderInfos.SceneSetupHandler.SetupNewMaterial(CurrentShaderInfos.Material);
    }

    private void ResetScene(AsyncOperation _)
    {
        CurrentShaderInfos.SceneSetupHandler.OnSceneExit();
        CurrentShaderInfos.CameraHandler.OnSceneExit();

        CurrentShaderInfos = null;

        mainCamera.gameObject.SetActive(true);
        TelaDeEscolha.Instance.transform.parent.gameObject.SetActive(true);
        AnimationBackgroundObjects.Instance.gameObject.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        if (CurrentShaderInfos)
            ResetScene(new());
    }
}
