using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement: MonoBehaviour
{
    private Camera my_camera;
    bool dragingScreen = false;
    [SerializeField] private float dragingScreenSpeed;
    Vector2 dragingScreenPosition;

    private void Awake() {
        my_camera = GetComponent<Camera>();
    }

    public void StartMoving() {
        dragingScreen = true;
        dragingScreenPosition = Mouse.current.position.ReadValue();
    }

    public void EndMoving() {
        dragingScreen = false;
    }

    public void Moving() {
        Vector3 NewCameraPosition = Camera.main.ScreenToViewportPoint((Mouse.current.position.ReadValue() - dragingScreenPosition) * dragingScreenSpeed);

        dragingScreenPosition = Mouse.current.position.ReadValue();

        my_camera.transform.position -= NewCameraPosition;
    }

    public bool IsDraggingScreen() {
        return dragingScreen;
    }
}
