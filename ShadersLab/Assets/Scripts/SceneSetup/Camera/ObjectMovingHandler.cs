using System;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "ObjectMovingHandler", menuName = "ScriptableObjects/Scene Setup/Camera/ObjectMovingHandler")]
public class ObjectMovingHandler : CameraHandler
{
    [SerializeField] private float defaultHoverDistance = 5f;
    [SerializeField] private MinMax hoverDistanceLimits = new() { Min = 1f, Max = 10f };
    [SerializeField] private MouseKey holdKey;
    [SerializeField] private float mouseDragMultiplier = 0.5f;
    [SerializeField] private KeyCode resetPositionKey = KeyCode.R;

    private GameObject currentObject;

    private bool mouseIsBeingHold;

    public override void OnSceneStart()
    {
        Camera.main.transform.position = new(defaultHoverDistance, 0f, 0f);

        Camera.main.transform.LookAt(Vector3.zero);

        currentObject = ObjectManager.CurrentObject;

        currentObject.transform.position = Vector3.zero;

        currentObject.transform.rotation = Quaternion.identity;
    }

    public override void OnSceneExit() { }

    public override void OnAplicationExit() { }

    public override void OnUpdate()
    {
        if (!currentObject)
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
            currentObject.transform.RotateAround(currentObject.transform.position, Vector3.up, Input.mousePositionDelta.x * mouseDragMultiplier * (1920f / Screen.width));
            currentObject.transform.RotateAround(currentObject.transform.position, currentObject.transform.right, -Input.mousePositionDelta.y * mouseDragMultiplier * (1080f / Screen.height));
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            float hoverDistance = Mathf.Clamp(currentObject.transform.position.x - Input.mouseScrollDelta.y, hoverDistanceLimits.Min, hoverDistanceLimits.Max);

            currentObject.transform.position = Vector3.right * hoverDistance;
        }

        if (Input.GetKeyDown(resetPositionKey))
        {
            currentObject.transform.position = Vector3.zero;
            currentObject.transform.rotation = Quaternion.identity;
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
