using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public static Graph Instance;

    public event EventHandler<OnSelectedGraphObjectChangedEventargs> OnSelectedGraphObjectChanged;

    public class OnSelectedGraphObjectChangedEventargs : EventArgs {
        public GraphObject selectedGraphObject;
    }

    [SerializeField] private Transform vertexPrefab;
    [SerializeField] private Transform edgeEndPoint;
    [SerializeField] private Transform edgePrefab;

    private List<Vertex> vertexes;
    private GraphObject selectedGraphObject;
    private Vertex clickedVertex = null;
    private Vertex lastClickedVertex = null;
    private Edge temporaryEdge = null;
    private Vertex startingVertex = null;
    private float clickStartTime;
    private float clickHoldTime = 0.2f;
    private float movingDistance = 0.3f;
    private bool offsetedByMovingDistance = false;
    private Vector3 startPositionClickedVertex;
    private Vector3 mousePositionOnClick;

    

    private void Awake() {
        Instance = this;
        vertexes = new List<Vertex>();
    }

    public void SpawnVertex(Vector3 spawnPosition) {
        Transform vertexTransform = Instantiate(vertexPrefab);

        vertexTransform.parent = transform;
        vertexTransform.GetComponent<Vertex>().SetLocalPosition(spawnPosition);
        vertexes.Add(vertexTransform.GetComponent<Vertex>());
        vertexTransform.GetComponent<Vertex>().SetNumber(vertexes.Count);
    }
    
    public void HandleSelection(UnityEngine.RaycastHit2D rayHit) {
        if (rayHit.collider != null) {
            if (rayHit.transform.TryGetComponent(out GraphObject graphObject)) {
                if (selectedGraphObject != graphObject) {
                    SetSelectedGraphObject(graphObject);
                }
            } else {
                SetSelectedGraphObject(null);
            }
        } else {
            SetSelectedGraphObject(null);
        }
    }

    private void SetSelectedGraphObject(GraphObject selectedGraphObject) {
        this.selectedGraphObject = selectedGraphObject;
        if (OnSelectedGraphObjectChanged != null) {
            OnSelectedGraphObjectChanged(this, new OnSelectedGraphObjectChangedEventargs {
                selectedGraphObject = selectedGraphObject
            }) ;
        }
    }

    public void ClickStart(UnityEngine.RaycastHit2D rayHit) {
        if (rayHit.collider != null) {
            if (rayHit.transform.TryGetComponent(out Vertex clickedVertex)) {
                this.clickedVertex = clickedVertex;
                clickStartTime = Time.time;
                mousePositionOnClick = InputHandler.Instance.GetMousePosition();
                offsetedByMovingDistance = false;
                startPositionClickedVertex = clickedVertex.GetPosition();
                startPositionClickedVertex.z = 0f;
            }
        }
    }

    public void ClickEnd(UnityEngine.RaycastHit2D rayHit) {
        if (rayHit.collider != null && rayHit.transform.TryGetComponent(out Vertex clickedVertex) && !movingVertex()) {
            if (lastClickedVertex == null) {
                if (this.clickedVertex == clickedVertex) {
                    lastClickedVertex = clickedVertex;
                    Transform edgeTransform = Instantiate(edgePrefab);
                    edgeTransform.parent = transform;
                    temporaryEdge = edgeTransform.GetComponentInChildren<Edge>();
                    temporaryEdge.SpawnEdge(lastClickedVertex.transform, edgeEndPoint);
                    return;
                }
            } else {
                if (clickedVertex != lastClickedVertex) {
                    temporaryEdge.SetB(clickedVertex.transform);
                    temporaryEdge.AddEdgeToVertexes();
                } else {
                    Destroy(temporaryEdge.gameObject);
                }
            }
        } else {
            if (temporaryEdge != null) {
                Destroy(temporaryEdge.gameObject);
            }
        }
        lastClickedVertex = null;
        temporaryEdge = null;
        this.clickedVertex = null;
    }

    public void SetClickedVertexPosition(Vector3 position) {
        clickedVertex.SetLocalPosition(position - (mousePositionOnClick - startPositionClickedVertex));
    }

    public bool movingVertex() {
        if (clickedVertex != null && !isTemporaryEdge() && (InputHandler.Instance.GetMousePosition() - mousePositionOnClick).magnitude >= movingDistance){
            offsetedByMovingDistance = true;
        }
        return clickedVertex != null && !isTemporaryEdge() &&(Time.time - clickStartTime >= clickHoldTime || offsetedByMovingDistance);
    }

    public bool isTemporaryEdge() {
        return temporaryEdge != null;
    }

    public void SetEdgeEndPoint(Vector3 position) {
        edgeEndPoint.position = position;
    }

    public void Delete() {
        if (selectedGraphObject != null){
            selectedGraphObject.Delete();
            if (selectedGraphObject.TryGetComponent(out Vertex deletedVertex)) {
                vertexes.Remove(deletedVertex);
                RedoNumberOnVertexes();
            }
        }
    }

    private void RedoNumberOnVertexes() {
        for (int i = 0; i < vertexes.Count; i++) {
            vertexes[i].SetNumber(i + 1);
        }
    }

    public void UpdateStartingVertex() {
        if (selectedGraphObject != null && selectedGraphObject.TryGetComponent<Vertex>(out Vertex selectedVertex)){
            if (selectedVertex != startingVertex) {
                if (startingVertex != null) {
                    startingVertex.UnsetStarting();
                }
                startingVertex = selectedVertex;
                startingVertex.SetStarting();
            }
        }
    }

    public Vertex GetStartingVertex() {
        return startingVertex;
    }

    public int NumberOfVertexes() {
        return vertexes.Count;
    }

    public List<Vertex> GetAllVertexes() {
        return vertexes;
    }
}
