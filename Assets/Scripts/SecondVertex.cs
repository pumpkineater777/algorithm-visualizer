using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondVertex : MonoBehaviour
{
    private int number;
    private SecondVertexVisual secondVertexVisual;
    private bool visited = false;

    private void Awake() {
        secondVertexVisual = GetComponent<SecondVertexVisual>();
    }
    public void SetNumber(int number) {
        this.number = number;
        secondVertexVisual.SetNumberText(number);
    }

    public void SetCoords(Vector3 coords) {
        transform.position = new Vector3(coords.x, coords.y, transform.position.z);
        SetVisited(true);
    }
    public bool IsVisited() { 
        return visited; 
    }

    public void SetVisited(bool visited) {
        this.visited = visited;
    }

    public void UpdateVisual(Vertex.VertexGameState vertexGameState) {
        secondVertexVisual.UpdateVisual(vertexGameState);
    }
}
