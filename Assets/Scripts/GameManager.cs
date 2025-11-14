using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics.Tracing;
using Unity.Properties;
using UnityEditor.SceneTemplate;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    private Vertex currentVertex;
    public struct Step {
        public Edge edge;
        public Vertex vertex;
    }

    public enum AlgorithmState {
        setUp,
        vertexOverview,
        checkingEdge,
        vertexEnd,
        end,
    }

    private List<Vertex> used;
    private List<Vertex> parent;
    private List<Vertex> order;
    private List<Step> steps;
    private AlgorithmState algorithmState;

    private void Awake() {
        Instance = this;

        algorithmState = AlgorithmState.setUp;
    }

    public void NextStep() {
        switch(algorithmState) {
            case AlgorithmState.vertexOverview:

                foreach (var e in currentVertex.GetEdges()) {
                    if (!e.Visited(currentVertex)) {
                        e.Visit(currentVertex);

                        steps.Add(new Step {
                            edge = e,
                            vertex = e.GetAnotherVertex(currentVertex)
                        });

                        algorithmState = AlgorithmState.checkingEdge;

                        CommentsUI.Instance.UpdateComments(algorithmState);

                        return;
                    }
                }

                algorithmState = AlgorithmState.vertexEnd;

                break;

            case AlgorithmState.checkingEdge:

                steps.Last().edge.Unchecking();

                if (!used.Contains(steps.Last().vertex)){
                    currentVertex.SetInactive();

                    parent[steps.Last().vertex.GetIndex()] = currentVertex;
                    used.Add(steps.Last().vertex);

                    currentVertex = steps.Last().vertex;

                    currentVertex.SetActive();

                    order.Add(currentVertex);

                    algorithmState = AlgorithmState.vertexOverview;

                    break;
                }

                foreach (var e in currentVertex.GetEdges()) {
                    if (!e.Visited(currentVertex)) {
                        e.Visit(currentVertex);

                        steps.Add(new Step {
                            edge = e,
                            vertex = e.GetAnotherVertex(currentVertex)
                        });

                        algorithmState = AlgorithmState.checkingEdge;

                        CommentsUI.Instance.UpdateComments(algorithmState);

                        return;
                    }
                }

                algorithmState = AlgorithmState.vertexEnd;

                break;
            case AlgorithmState.vertexEnd:
                if (parent[currentVertex.GetIndex()] == null) {
                    currentVertex.SetInactive();

                    algorithmState = AlgorithmState.end;
                    break;
                }

                currentVertex.SetInactive();

                currentVertex = parent[currentVertex.GetIndex()];

                currentVertex.SetActive();

                order.Add(currentVertex);

                algorithmState = AlgorithmState.vertexOverview;

                break;
            case AlgorithmState.end:
                break;
        }

        CommentsUI.Instance.UpdateComments(algorithmState);
    }

    public void PrevStep() {
        switch(algorithmState) {
            case AlgorithmState.vertexOverview:
                if (used.Count == 1) {
                    break;
                }

                order.RemoveAt(order.Count - 1);

                if (used.Last() == currentVertex) {

                    currentVertex.SetInactive();
                    currentVertex = order.Last();
                    currentVertex.SetActive();

                    used.RemoveAt(used.Count - 1);

                    steps.Last().edge.Visit(currentVertex);

                    algorithmState = AlgorithmState.checkingEdge;

                    break;
                }

                currentVertex.SetInactive();
                currentVertex = order.Last();
                currentVertex.SetActive();

                algorithmState = AlgorithmState.vertexEnd;

                break;
            case AlgorithmState.checkingEdge:

                steps.Last().edge.Unvisit(currentVertex);
                steps.RemoveAt(steps.Count - 1);

                if (steps.Count >= 1 && steps.Last().edge.GetAnotherVertex(steps.Last().vertex) == currentVertex) {
                    steps.Last().edge.Visit(currentVertex);

                    break;
                }

                algorithmState = AlgorithmState.vertexOverview;

                break;
            case AlgorithmState.vertexEnd:
                if (steps.Count >= 1 && steps.Last().edge.GetAnotherVertex(steps.Last().vertex) == currentVertex) {
                    steps.Last().edge.Visit(currentVertex);

                    algorithmState = AlgorithmState.checkingEdge;

                    break;
                }
                algorithmState = AlgorithmState.vertexOverview;
                break;
            case AlgorithmState.end:
                currentVertex = order.Last();

                currentVertex.SetActive();

                algorithmState = AlgorithmState.vertexEnd;
                break;
        }

        CommentsUI.Instance.UpdateComments(algorithmState);
    }

    public Vertex GetCurrentVertex() {
        return currentVertex;
    }

    public Step LastStep() {
        return steps.Last();
    }

    public bool IsVertexVisited(Vertex v) {
        return used.Contains(v);
    }

    public List<Vertex> VisitedVertexes() {
        return used;
    }

    public List<Vertex> UnvisitedVertexes() {
        List<Vertex> allVertexes = Graph.Instance.GetAllVertexes();

        List<Vertex> unmarkedVertexes = new List<Vertex>();

        foreach (Vertex v in allVertexes) {
            if (!used.Contains(v)) {
                unmarkedVertexes.Add(v);
            }
        }
        return unmarkedVertexes;
    }

    public void StartAlgorithm() {
        used = new List<Vertex>();
        steps = new List<Step>();
        order = new List<Vertex>();

        currentVertex = Graph.Instance.GetStartingVertex();

        used.Add(currentVertex);
        order.Add(currentVertex);

        parent = new List<Vertex>(new Vertex[Graph.Instance.NumberOfVertexes()]);
        parent[currentVertex.GetIndex()] = null;

        foreach (var e in Graph.Instance.GetAllVertexes()) {
            e.SetInactive();
        }

        algorithmState = AlgorithmState.vertexOverview;

        used.Last().SetActive();

        CommentsUI.Instance.UpdateComments(algorithmState);
    }

    public void EndAlgorithm() {
        foreach (var v in Graph.Instance.GetAllVertexes()) {
            v.ToSetUp();
        }

        algorithmState = AlgorithmState.setUp;

        Graph.Instance.GetStartingVertex().SetStarting();

        CommentsUI.Instance.UpdateComments(algorithmState);
    }
}
