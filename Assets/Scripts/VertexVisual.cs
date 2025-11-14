using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class VertexVisual : MonoBehaviour
{
    [SerializeField] private Vertex vertex;
    [SerializeField] private GameObject activeVertexVisual;
    [SerializeField] private GameObject setUpVertexVisual;
    [SerializeField] private GameObject inactiveVertexVisual;
    [SerializeField] private TextMeshProUGUI numberText;

    private const string INACTIVATED = "Inactivated";
    private const string ACTIVATED = "Activated";

    private Animator animator;
    bool activated = false;
    bool inactivated = false;

    public void Awake() {
        animator = GetComponent<Animator>();
    }

    public void SetNumberText(int number) {
        numberText.text = number.ToString();
    }

    public void VertexUnsetStarting() {
        setUpVertexVisual.SetActive(true);
        activeVertexVisual.SetActive(false);
    }

    public void VertexSetStarting() {
        activeVertexVisual.SetActive(true);
        setUpVertexVisual.SetActive(false);
    }

    public void UpdateVisual(Vertex.VertexGameState vertexGameState) {
        transform.localScale = Vector3.one;

        activeVertexVisual.SetActive(false);
        setUpVertexVisual.SetActive(false);
        inactiveVertexVisual.SetActive(false);

        switch (vertexGameState) {
            case Vertex.VertexGameState.active:
                activeVertexVisual.SetActive(true);
                transform.localScale = new Vector3(1.1f, 1.1f, 1f);

                if (inactivated) {
                    animator.SetTrigger(ACTIVATED);
                }
                activated = true;
                inactivated = false;

                break;
            case Vertex.VertexGameState.inactive:
                inactiveVertexVisual.SetActive(true);

                if (activated) {
                    animator.SetTrigger(INACTIVATED);
                }
                inactivated = true;
                activated = false;

                break;
            case Vertex.VertexGameState.setUp:
                setUpVertexVisual.SetActive(true);

                if (activated) {
                    animator.SetTrigger(INACTIVATED);
                }

                activated = false;
                inactivated = false;
                break;
        }
    }
}
