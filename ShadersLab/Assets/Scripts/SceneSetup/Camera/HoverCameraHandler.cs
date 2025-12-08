using System;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "HoverCameraHandler", menuName = "ScriptableObjects/Scene Setup/Camera/HoverCameraHandler")]
public class HoverCameraHandler : CameraHandler
{
    [SerializeField] private float defaultHoverDistance = 5f;
    [SerializeField] private MinMax hoverDistanceLimits = new() { Min = 1f, Max = 10f };
    [SerializeField] private MouseKey holdKey;
    [SerializeField] private float mouseDragMultiplier = 0.5f;
    [SerializeField] private KeyCode resetPositionKey = KeyCode.R;

    private Camera camera;

    private bool mouseIsBeingHold;

    public override void OnSceneStart()
    {
        camera = Camera.main;

        camera.transform.position = new(defaultHoverDistance, 0f, 0f);

        camera.transform.LookAt(Vector3.zero);
    }

    public override void OnSceneExit() { }

    public override void OnAplicationExit() { }

    public override void OnUpdate()
    {
        if (!camera)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (LabelDragger.IsBeingDragged)
            return;

        if (!mouseIsBeingHold && Input.GetMouseButton((int)holdKey))
            mouseIsBeingHold = true;

        if (mouseIsBeingHold && !Input.GetMouseButton((int)holdKey))
            mouseIsBeingHold = false;

        if (mouseIsBeingHold)
        {
            camera.transform.RotateAround(Vector3.zero, Vector3.up, Input.mousePositionDelta.x * mouseDragMultiplier);
            camera.transform.RotateAround(Vector3.zero, camera.transform.right, -Input.mousePositionDelta.y * mouseDragMultiplier);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            float hoverDistance = Mathf.Clamp(camera.transform.position.magnitude - Input.mouseScrollDelta.y, hoverDistanceLimits.Min, hoverDistanceLimits.Max);

            camera.transform.position = camera.transform.position.normalized * hoverDistance;
        }

        if (Input.GetKeyDown(resetPositionKey))
        {
            camera.transform.position = new(defaultHoverDistance, 0f, 0f);
            camera.transform.LookAt(Vector3.zero);
        }
    }

    enum MouseKey
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    [Serializable]
    struct MinMax
    {
        public float Min;
        public float Max;
    }
}
