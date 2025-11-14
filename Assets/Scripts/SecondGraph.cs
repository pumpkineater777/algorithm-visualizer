using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecondGraph : MonoBehaviour
{
    [SerializeField] private Transform secondVertexPrefab;
    [SerializeField] private Transform secondEdgePrefab;

    public static SecondGraph Instance;

    private List<SecondVertex> vertexes;
    private List<SecondEdge> edges;
    private Vector3 rootVertexCoords = new Vector3(-35f, 4f, 0f);
    private float offsetY = 1.4f;
    private float offsetX = 1.2f;
    private List<List<int>> graph;

    private void Awake() {
        Instance = this;
    }

    public void StartAlgorithm(Vertex startingVertex, List<Vertex> visited) {
        vertexes = new List<SecondVertex>(new SecondVertex[Graph.Instance.NumberOfVertexes()]);
        edges = new List<SecondEdge>(new SecondEdge[Graph.Instance.NumberOfVertexes()]);

        UpdateVisual(startingVertex, visited);
    }

    public void UpdateVisual(Vertex startingVertex, List<Vertex> visited) {
        graph = new List<List<int>>();
        for (int i = 0; i < Graph.Instance.NumberOfVertexes(); i++) {
            graph.Add(new List<int>());
        }

        List<int> used = new List<int>(new int[Graph.Instance.NumberOfVertexes()]);

        SetGraph(startingVertex, graph, used, visited);

        List<int> widths = new List<int>();
        for (int i = 0; i < Graph.Instance.NumberOfVertexes(); i++) {
            widths.Add(1);
        }

        SetWidths(startingVertex.GetIndex(), graph, widths);

        SetVisuals(startingVertex.GetIndex(), -1, rootVertexCoords, graph, widths);

        foreach(var v in visited) {
            vertexes[v.GetIndex()].UpdateVisual(v.GetVertexGameState());
        }

        foreach (var v in vertexes) {
            if (v != null) {
                if (v.IsVisited()) {
                    v.SetVisited(false);
                } else {
                    Destroy(v.gameObject);
                }
            }
        }

        foreach (var e in edges) {
            if (e != null) {
                if (e.IsVisited()) {
                    e.SetVisited(false);
                } else {
                    Destroy(e.gameObject);
                }
            }
        }
    }

    public void EndAlgorithm() {
        foreach (var v in vertexes) {
            if (v != null) {
                Destroy(v.gameObject);
            }
        }

        foreach (var e in edges) {
            if (e != null) {
                Destroy(e.gameObject);
            }
        }
    }

    private void SetGraph(Vertex v, List<List<int>> graph, List<int> used, List<Vertex> visited) {
        used[v.GetIndex()] = 1;
        foreach (var e in v.GetEdges()) {
            Vertex u = e.GetAnotherVertex(v);
            if (used[u.GetIndex()] == 0 && visited.Contains(u)) {
                graph[v.GetIndex()].Add(u.GetIndex());
                SetGraph(u, graph, used, visited);
            }
        }
    }

    private void SetWidths(int v, List<List<int>> graph, List<int>widths) {
        int count = 0;
        foreach (int u in graph[v]) {
            SetWidths(u, graph, widths);
            count += widths[u];
        }
        if (count > 0) {
            widths[v] = count;
        }
    }

    private float GetOffsetVertex(int v, List<List<int>> graph, List<int> widths) {
        if (graph[v].Count == 0) {
            return 0;
        }
        float count = 0;
        for (int i = 0; i < (graph[v].Count - 1) / 2; i++) {
            count += widths[graph[v][i]];
        }
        if (graph[v].Count % 2 == 0) {
            count += (widths[graph[v][graph[v].Count / 2]] + widths[graph[v][graph[v].Count / 2 - 1]]) / (float)2;
        } else {
            count += widths[graph[v][graph[v].Count / 2]] / (float)2;
        }
        return count;
    }

    private void SetVisuals(int v, int p, Vector3 coords, List<List<int>> graph, List<int> widths) {
        if (vertexes[v] == null) {
            vertexes[v] = Instantiate(secondVertexPrefab).GetComponent<SecondVertex>();
            vertexes[v].transform.parent = transform;
            vertexes[v].SetNumber(v + 1);
            vertexes[v].SetCoords(coords);
        } else {
            vertexes[v].SetCoords(coords);
        }

        if (p != -1) {
            if (edges[v] == null) {
                edges[v] = Instantiate(secondEdgePrefab).GetComponent<SecondEdge>();
                edges[v].SpawnEdge(vertexes[p], vertexes[v]);
            } else {
                edges[v].UpdateVisual();
            }
        }

        coords.x -= GetOffsetVertex(v, graph, widths) * offsetX;
        coords.y -= offsetY;

        foreach (int u in graph[v]) {
            coords.x += widths[u] * offsetX / 2;
            SetVisuals(u, v, coords, graph, widths);
            coords.x += widths[u] * offsetX / 2;
        }
    }
}
