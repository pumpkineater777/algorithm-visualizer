using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecondVertexVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private GameObject activeSecondVertexVisual;
    [SerializeField] private GameObject inactiveSecondVertexVisual;

    private const string INACTIVATED = "Inactivated";
    private const string ACTIVATED = "Activated";

    private Animator animator;
    bool activated = false;
    bool inactivated = true;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void SetNumberText(int number) {
        numberText.text = number.ToString();
    }

    public void UpdateVisual(Vertex.VertexGameState vertexGameState) {
        transform.localScale = Vector3.one;

        activeSecondVertexVisual.SetActive(false);
        inactiveSecondVertexVisual.SetActive(false);

        switch (vertexGameState) {
            case Vertex.VertexGameState.active:
                activeSecondVertexVisual.SetActive(true);
                transform.localScale = new Vector3(1.1f, 1.1f, 1f);

                if (inactivated) {
                    animator.SetTrigger(ACTIVATED);
                }
                activated = true;
                inactivated = false;

                break;
            case Vertex.VertexGameState.inactive:
                inactiveSecondVertexVisual.SetActive(true);

                if (activated) {
                    animator.SetTrigger(INACTIVATED);
                }
                inactivated = true;
                activated = false;

                break;
        }
    }
}
