using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;

public class Edge : GraphObject
{
    private Transform A, B;
    private Vertex Av = null, Bv = null;
    private BoxCollider2D boxCollider2D;
    private EdgeVisual edgeVisual;

    private EdgeGameState edgeGameState;

    private bool visitedA = false;
    private bool visitedB = false;

    public enum EdgeGameState {
        setUp,
        inactive,
        visited,
        unvisited,
        checking
    }

    private void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        edgeVisual = GetComponent<EdgeVisual>();
        boxCollider2D.enabled = false;
    }

    private void Start() {
        edgeGameState = EdgeGameState.setUp;

        edgeVisual.UpdateVisual(edgeGameState);
    }

    public void SpawnEdge(Transform A, Transform B) {
        this.A = A;
        this.B = B;
    }

    public void SetB(Transform B) {
        this.B = B;
    }

    override public void Delete() {
        Av.DeleteEdgeFromEdges(this);
        Bv.DeleteEdgeFromEdges(this);
        Destroy(gameObject);
    }

    public void DeleteExceptVertex(Vertex vertex) {
        if (Av != vertex) {
            Av.DeleteEdgeFromEdges(this);
        }
        if (Bv != vertex) {
            Bv.DeleteEdgeFromEdges(this);
        }
        Destroy(gameObject);
    }

    public void AddEdgeToVertexes() {
        Av = A.GetComponent<Vertex>();
        Bv = B.GetComponent<Vertex>();
        boxCollider2D.enabled = true;

        if (!Av.AreVertexesConnected(Bv)) {
            Av.AddEdge(this);
            Bv.AddEdge(this);
        } else {
            Destroy(gameObject);
        }
    }

    public Vertex GetAnotherVertex(Vertex vertex) {
        if (Av != vertex) {
            return Av;
        }
        return Bv;
    }

    public Vector3 Aposition() {
        if (Av == null) {
            return A.position;
        }
        return Av.transform.position;
    }

    public Vector3 Bposition() {
        if (Bv == null) {
            return B.position;
        }
        return Bv.transform.position;
    }

    public void SetInactive() {
        edgeGameState = EdgeGameState.inactive;

        edgeVisual.UpdateVisual(edgeGameState);
    }

    public void SetActive(Vertex v) {
        if (v == Av && visitedA) {
            edgeGameState = EdgeGameState.visited;

            edgeVisual.UpdateVisual(edgeGameState);

            return;
        }
        if (v == Bv && visitedB) {
            edgeGameState = EdgeGameState.visited;

            edgeVisual.UpdateVisual(edgeGameState);

            return;
        }

        edgeGameState = EdgeGameState.unvisited;

        edgeVisual.UpdateVisual(edgeGameState);
    }

    public bool Visited(Vertex v) {
        if (v == Av && visitedA) {
            return true;
        }
        if (v == Bv && visitedB) {  
            return true;
        }
        return false;
    }

    public void Visit(Vertex v) {
        if (v == Av) {
            edgeVisual.UpdateCheckingEdgeMaterial(true);
            visitedA = true;
        }
        if (v == Bv) {
            edgeVisual.UpdateCheckingEdgeMaterial(false);
            visitedB = true;
        }

        edgeGameState = EdgeGameState.checking;

        edgeVisual.UpdateVisual(edgeGameState);
    }

    public void Unvisit(Vertex v) {
        if (v == Av) {
            visitedA = false;
        }
        if (v == Bv) {
            visitedB = false;
        }

        edgeGameState = EdgeGameState.unvisited;

        edgeVisual.UpdateVisual(edgeGameState);
    }

    public void Unchecking() {
        edgeGameState = EdgeGameState.visited;

        edgeVisual.UpdateVisual(edgeGameState);
    }

    public bool IsVertexContained(Vertex v) {
        return (v == Av || v == Bv);
    }

    public void ToSetUp() {
        visitedA = false;
        visitedB = false;
        edgeGameState = EdgeGameState.setUp;

        edgeVisual.UpdateVisual(edgeGameState);
    }
}
