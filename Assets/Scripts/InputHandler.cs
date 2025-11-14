using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private CameraMovement secondCameraMovement;
    [SerializeField] private Camera secondCamera;
    private Camera mainCamera;
    public static InputHandler Instance;
    private PlayerInput playerInput;

    private void Awake() {
        Instance = this;
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnClickSetUp(InputAction.CallbackContext context) {

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (context.started) {
            if (rayHit.collider == null) {
                cameraMovement.StartMoving();
            } else {
                Graph.Instance.ClickStart(rayHit);
            }
        }
        if (context.canceled) {
            if (! cameraMovement.IsDraggingScreen()) {
                Graph.Instance.ClickEnd(rayHit);
            } else {
                cameraMovement.EndMoving();
            }
        }
    }


    public void OnCLickShowcase(InputAction.CallbackContext context) {
        if (context.started) {
            if (secondCamera.pixelRect.Contains(Mouse.current.position.ReadValue())) {
                secondCameraMovement.StartMoving();
            } else {
                cameraMovement.StartMoving();
            }
        }
        if (context.canceled) {
            if (secondCameraMovement.IsDraggingScreen()) {
                secondCameraMovement.EndMoving();
            } else if (cameraMovement.IsDraggingScreen()){
                cameraMovement.EndMoving();
            }
        }
    }

    public void OnRightClick(InputAction.CallbackContext context) {
        if (!context.started) { return; }

        if (cameraMovement.IsDraggingScreen()) { return; }

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (!rayHit.collider) {

            Graph.Instance.SpawnVertex(GetMousePosition());
        }
    }

    public void OnDelete(InputAction.CallbackContext context) {
        if (!context.started) { return; }
        
        if (cameraMovement.IsDraggingScreen()) { return; }

        Graph.Instance.Delete();
    }

    public void OnUpdateStartingVertex(InputAction.CallbackContext context) {
        if (!context.started) {
            return;
        }

        if (cameraMovement.IsDraggingScreen()) { return; }

        Graph.Instance.UpdateStartingVertex();
    }

    private void Update() {
        if (secondCameraMovement.IsDraggingScreen()) {

            secondCameraMovement.Moving();

        } else if (cameraMovement.IsDraggingScreen()) {

            cameraMovement.Moving();

        } else if (Graph.Instance.movingVertex()) {

            Graph.Instance.SetClickedVertexPosition(GetMousePosition());
        } else {
            var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));

            Graph.Instance.HandleSelection(rayHit);

            Graph.Instance.SetEdgeEndPoint(GetMousePosition());
        }
    }

    public Vector3 GetMousePosition() {
        Vector3 currentMousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        currentMousePosition.z = 0f;
        return currentMousePosition;
    }

    public void EndSetUpAlgorithm() {
        playerInput.actions.FindActionMap("SetUpAlgorithm").Disable();
        playerInput.actions.FindActionMap("ShowcaseAlgorithm").Enable();
    }

    public void EndAlgorithm() {
        playerInput.actions.FindActionMap("ShowcaseAlgorithm").Disable();
        playerInput.actions.FindActionMap("SetUpAlgorithm").Enable();
    }

    public void NextStep(InputAction.CallbackContext context) {
        GameManager.Instance.NextStep();
    }

    public void PrevStep(InputAction.CallbackContext context) {
        GameManager.Instance.PrevStep();
    }
}
