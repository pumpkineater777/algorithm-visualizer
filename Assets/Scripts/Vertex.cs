using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Vertex : GraphObject {
    private List<Edge> edges;
    private int number;
    private VertexGameState vertexGameState;
    private VertexVisual vertexVisual;

    public enum VertexGameState {
        setUp,
        active,
        inactive
    }

    private void Awake() {
        edges = new List<Edge>();
        vertexVisual = GetComponent<VertexVisual>();
    }

    private void Start() {
        vertexGameState = VertexGameState.setUp;

        vertexVisual.UpdateVisual(vertexGameState);
    }

    public void SetLocalPosition(Vector3 position) {
        transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
    }

    override public void Delete() {
        foreach (var edge in edges) {
            edge.DeleteExceptVertex(this);
        }
        Destroy(gameObject);
    }

    public void DeleteEdgeFromEdges(Edge edge) {
        edges.Remove(edge);
    }

    public void AddEdge(Edge edge) {
        edges.Add(edge);
    }

    public bool AreVertexesConnected(Vertex vertex) {
        foreach (var edge in edges) {
            if (edge.GetAnotherVertex(this) == vertex) {
                return true;
            }
        }
        return false;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public List<Edge> GetEdges() {
        return edges;
    }

    public List<Vertex> GetVertexes() {
        List<Vertex> vertexes = new List<Vertex>();
        foreach (var edge in edges) {
            vertexes.Add(edge.GetAnotherVertex(this));
        }
        return vertexes;
    }

    public void SetActive() {
        vertexGameState = VertexGameState.active;

        foreach (var e in edges) {
            e.SetActive(this);
        }

        vertexVisual.UpdateVisual(vertexGameState);
    }

    public void SetInactive() {
        vertexGameState = VertexGameState.inactive;

        foreach (var e in edges) {
            e.SetInactive();
        }

        vertexVisual.UpdateVisual(vertexGameState);
    }

    public void SetNumber(int number) {
        this.number = number;
        vertexVisual.SetNumberText(number);
    }

    public int GetNumber() {
        return number;
    }

    public int GetIndex() {
        return number - 1;
    }

    public void SetStarting() {
        vertexVisual.VertexSetStarting();
    }

    public void UnsetStarting() {
        vertexVisual.VertexUnsetStarting();
    }

    public void ToSetUp() {
        vertexGameState = VertexGameState.setUp;

        foreach (var e in edges) {
            e.ToSetUp();
        }

        vertexVisual.UpdateVisual(vertexGameState);
    }

    public List<Vertex> GetVisitedVertexes() {
        var vertexes = new List<Vertex>();
        foreach (var e in edges) {
            if (e.Visited(this)){
                vertexes.Add(e.GetAnotherVertex(this));
            }
        }
        return vertexes;
    }
    public List<Vertex> GetUnvisitedVertexes() {
        var vertexes = new List<Vertex>();
        foreach (var e in edges) {
            if (! e.Visited(this)) {
                vertexes.Add(e.GetAnotherVertex(this));
            }
        }
        return vertexes;
    }

    public VertexGameState GetVertexGameState() {
        return vertexGameState;
    }
}
