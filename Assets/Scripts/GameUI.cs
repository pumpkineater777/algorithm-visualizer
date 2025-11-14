using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [SerializeField] private Button nextStepButton;
    [SerializeField] private Button prevStepButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject nextStepUI;
    [SerializeField] private GameObject prevStepUI;
    [SerializeField] private GameObject exitUI;
    [SerializeField] private GameObject startUI;
    [SerializeField] private Camera SecondCamera;

    private void Awake() {
        nextStepUI.gameObject.SetActive(false);
        prevStepUI.gameObject.SetActive(false);
        exitUI.gameObject.SetActive(false);
        SecondCamera.enabled = false;
    }

    private void Start() {
        nextStepButton.onClick.AddListener(NextStep);
        prevStepButton.onClick.AddListener(PrevStep);
        startButton.onClick.AddListener(StartAlgorithm);
        exitButton.onClick.AddListener(EndAlgorithm);
    }

    private void StartAlgorithm() {
        if (Graph.Instance.GetStartingVertex() == null) {
            CommentsUI.Instance.StartWithoutStartingVertex();
            return;
        }

        startUI.gameObject.SetActive(false);
        nextStepUI.gameObject.SetActive(true);
        prevStepUI.gameObject.SetActive(true);
        exitUI.gameObject.SetActive(true);
        SecondCamera.enabled = true;

        GameManager.Instance.StartAlgorithm();

        InputHandler.Instance.EndSetUpAlgorithm();

        SecondGraph.Instance.StartAlgorithm(Graph.Instance.GetStartingVertex(), GameManager.Instance.VisitedVertexes());
    }

    private void EndAlgorithm() {
        startUI.gameObject.SetActive(true);
        nextStepUI.gameObject.SetActive(false);
        prevStepUI.gameObject.SetActive(false);
        exitUI.gameObject.SetActive(false);
        SecondCamera.enabled = false;

        GameManager.Instance.EndAlgorithm();

        SecondGraph.Instance.EndAlgorithm();

        InputHandler.Instance.EndAlgorithm();
    }

    private void NextStep() {
        GameManager.Instance.NextStep();

        SecondGraph.Instance.UpdateVisual(Graph.Instance.GetStartingVertex(), GameManager.Instance.VisitedVertexes());
    }

    private void PrevStep() {
        GameManager.Instance.PrevStep();

        SecondGraph.Instance.UpdateVisual(Graph.Instance.GetStartingVertex(), GameManager.Instance.VisitedVertexes());
    }
}
