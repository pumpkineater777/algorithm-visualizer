using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEdge : MonoBehaviour
{
    private SecondEdgeVisual secondEdgeVisual;
    private SecondVertex A;
    private SecondVertex B;
    private bool visited = false;

    private void Awake() {
        secondEdgeVisual = GetComponent<SecondEdgeVisual>();
    }

    public void SpawnEdge(SecondVertex A, SecondVertex B) {
        this.A = A;
        this.B = B;
        UpdateVisual();
    }

    public void UpdateVisual() {
        secondEdgeVisual.UpdateVisual(A.transform.position, B.transform.position);

        SetVisited(true);
    }

    public bool IsVisited() {
        return visited;
    }

    public void SetVisited(bool visited) {
        this.visited = visited;
    }
}
