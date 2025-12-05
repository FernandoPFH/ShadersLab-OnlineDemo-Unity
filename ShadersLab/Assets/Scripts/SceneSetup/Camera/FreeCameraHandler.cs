using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FreeCameraHandler", menuName = "ScriptableObjects/Scene Setup/Camera/FreeCameraHandler")]
public class FreeCameraHandler : CameraHandler
{
    [SerializeField] private KeyCode forwardMovementKey = KeyCode.W;
    [SerializeField] private KeyCode backMovementKey = KeyCode.S;
    [SerializeField] private KeyCode leftMovementKey = KeyCode.A;
    [SerializeField] private KeyCode rightMovementKey = KeyCode.D;
    [SerializeField] private float movementSpeedMultiplier = 1f;
    [SerializeField] private MouseKey holdKey;
    [SerializeField] private float rotationSpeedMultiplier = 1f;
    [SerializeField] private KeyCode resetPositionKey = KeyCode.R;
    private Camera camera;

    private bool mouseIsBeingHold;

    public override void OnSceneStart()
    {
        camera = Camera.main;

        camera.transform.position = Vector3.zero;
        camera.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public override void OnSceneExit() { }

    public override void OnAplicationExit() { }

    public override void OnUpdate()
    {
        if (!camera)
            return;

        if (ShaderEditorUI.IsUIBeingHover)
            return;

        if (!mouseIsBeingHold && Input.GetMouseButton((int)holdKey))
            mouseIsBeingHold = true;

        if (mouseIsBeingHold && !Input.GetMouseButton((int)holdKey))
            mouseIsBeingHold = false;

        if (mouseIsBeingHold)
        {
            camera.transform.RotateAround(camera.transform.position, Vector3.up, Input.mousePositionDelta.x * rotationSpeedMultiplier);
            camera.transform.RotateAround(camera.transform.position, camera.transform.right, -Input.mousePositionDelta.y * rotationSpeedMultiplier);
        }

        camera.transform.position += camera.transform.forward * (GetKeyDownAsInt(forwardMovementKey) - GetKeyDownAsInt(backMovementKey)) * movementSpeedMultiplier + camera.transform.right * (GetKeyDownAsInt(rightMovementKey) - GetKeyDownAsInt(leftMovementKey)) * movementSpeedMultiplier;

        if (Input.GetKeyDown(resetPositionKey))
        {
            camera.transform.position = Vector3.zero;
            camera.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    private int GetKeyDownAsInt(KeyCode key)
        => Convert.ToInt16(Input.GetKey(key));

    enum MouseKey
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }
}
