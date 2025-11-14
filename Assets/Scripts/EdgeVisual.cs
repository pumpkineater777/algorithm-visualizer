using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeVisual : MonoBehaviour
{
    [SerializeField] private Edge edge;
    [SerializeField] private float z;
    [SerializeField] private GameObject checkingEdgeVisual;
    [SerializeField] private GameObject visitedEdgeVisual;
    [SerializeField] private GameObject unvisitedEdgeVisual;
    [SerializeField] private GameObject inactiveEdgeVisual;
    [SerializeField] private GameObject setUpEdgeVisual;
    [SerializeField] private Material checkingEdgeMaterial;

    private float width;
    private float widthActive = 0.3f;
    private float widthNonActive = 0.25f;
    private bool setUp = true;
    private float TillingY = 3.33333f;

    private void Awake() {
        width = widthNonActive;
    }

    public void UpdateCheckingEdgeMaterial(bool Aparent) {
        if (Aparent) {
            checkingEdgeMaterial.SetVector("_Tiling", new Vector2(-1, TillingY));
        } else {
            checkingEdgeMaterial.SetVector("_Tiling", new Vector2(1, TillingY));
        }
    }

    public void UpdateVisual(Edge.EdgeGameState edgeGameState) {
        checkingEdgeVisual.transform.localScale = Vector3.one;

        checkingEdgeVisual.SetActive(false);
        visitedEdgeVisual.SetActive(false);
        inactiveEdgeVisual.SetActive(false);
        unvisitedEdgeVisual.SetActive(false);
        setUpEdgeVisual.SetActive(false);

        switch(edgeGameState) {
            case Edge.EdgeGameState.inactive:
                inactiveEdgeVisual.SetActive(true);

                setUp = false;
                width = widthNonActive;

                break;
            case Edge.EdgeGameState.setUp:
                setUpEdgeVisual.SetActive(true);

                setUp = true;
                width = widthNonActive;

                break;
            case Edge.EdgeGameState.checking:
                checkingEdgeVisual.SetActive(true);

                setUp = false;
                width = widthActive;

                break;
            case Edge.EdgeGameState.unvisited:
                unvisitedEdgeVisual.SetActive(true);

                setUp = false;
                width = widthActive;

                break;
            case Edge.EdgeGameState.visited:
                visitedEdgeVisual.SetActive(true);

                setUp = false;
                width = widthActive;

                break;
        }
    }

    private void Update() {
        Vector3 Apos = edge.Aposition(), Bpos = edge.Bposition();
        
        transform.localPosition = new Vector3((Apos.x + Bpos.x) / 2, (Apos.y + Bpos.y) / 2, z);

        transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(Apos.y - Bpos.y, Apos.x - Bpos.x));

        if (!setUp) {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(Mathf.Pow(Mathf.Pow(Apos.y - Bpos.y, 2) + Mathf.Pow(Apos.x - Bpos.x, 2), 0.5f), width, 1f), Time.deltaTime);
        } else {
            transform.localScale = new Vector3(Mathf.Pow(Mathf.Pow(Apos.y - Bpos.y, 2) + Mathf.Pow(Apos.x - Bpos.x, 2), 0.5f), width, 1f);
        }
    }
}
