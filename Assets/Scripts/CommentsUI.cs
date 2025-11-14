using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Text;
using Unity.VisualScripting;

public class CommentsUI : MonoBehaviour
{
    public static CommentsUI Instance;

    private CommmentsJSON commentsJSON;

    [SerializeField] TextMeshProUGUI commentsText;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        commentsJSON = JsonUtility.FromJson<CommmentsJSON>(File.ReadAllText(Application.dataPath + "/JSONs/temp.txt", Encoding.UTF8));

        commentsText.text = commentsJSON.SetUpText;
    }

    private string ListToString(List<Vertex> vertexes) {
        string ans = "";
        for (int i = 0; i < vertexes.Count - 1; i++) {
            ans += vertexes[i].GetNumber().ToString() + ", ";
        }
        ans += vertexes[vertexes.Count - 1].GetNumber().ToString();
        return ans;
    }

    public void UpdateComments(GameManager.AlgorithmState algorithmState) {
        Vertex currentVertex = GameManager.Instance.GetCurrentVertex();
        GameManager.Step currentStep;
        switch(algorithmState) {
            case GameManager.AlgorithmState.setUp:
                commentsText.text = commentsJSON.SetUpText;
                break;
            case GameManager.AlgorithmState.vertexOverview:
                commentsText.text = commentsJSON.VertexOverviewText.Vertex + currentVertex.GetNumber() + "\n\n";

                if (currentVertex.GetEdges().Count == 0) {
                    commentsText.text += commentsJSON.VertexOverviewText.NoEdges;
                    break;
                }

                if (currentVertex.GetVisitedVertexes().Count != 0) {
                    commentsText.text += commentsJSON.VertexOverviewText.VisitedVertexes + ListToString(currentVertex.GetVisitedVertexes()) + "\n\n";
                }

                if (currentVertex.GetUnvisitedVertexes().Count != 0) {
                    commentsText.text += commentsJSON.VertexOverviewText.UnvisitedVertexes + ListToString(currentVertex.GetUnvisitedVertexes()) + "\n\n";
                    commentsText.text += commentsJSON.VertexOverviewText.EdgesOrder;
                }

                break;
            case GameManager.AlgorithmState.checkingEdge:
                commentsText.text = commentsJSON.CheckingEdgeText.Vertex + currentVertex.GetNumber() + "\n\n";

                currentStep = GameManager.Instance.LastStep();

                commentsText.text += commentsJSON.CheckingEdgeText.Edge + currentStep.vertex.GetNumber() + "\n\n";

                commentsText.text += commentsJSON.CheckingEdgeText.AnotherVertex + currentStep.vertex.GetNumber();

                if (GameManager.Instance.IsVertexVisited(currentStep.vertex)) {
                    commentsText.text += commentsJSON.CheckingEdgeText.InvalidEdge;
                } else {
                    commentsText.text += commentsJSON.CheckingEdgeText.ValidEdge;
                }

                break;
            case GameManager.AlgorithmState.vertexEnd:
                commentsText.text = commentsJSON.VertexEndText.Vertex + currentVertex.GetNumber() + "\n\n";

                commentsText.text += commentsJSON.VertexEndText.NoUnvisitedVertexes + "\n\n";
                
                commentsText.text += commentsJSON.VertexEndText.ReturnToParent + "\n\n";

                break;
            case GameManager.AlgorithmState.end:
                commentsText.text = commentsJSON.EndText.Conclusion + "\n\n";

                commentsText.text += commentsJSON.EndText.VisitedVertexes + ListToString(GameManager.Instance.VisitedVertexes());

                if (GameManager.Instance.UnvisitedVertexes().Count > 0) {
                    commentsText.text += "\n\n" + commentsJSON.EndText.UnvisitedVertexes + ListToString(GameManager.Instance.UnvisitedVertexes());
                }

                break;
        }
    }

    public void StartWithoutStartingVertex() {
        commentsText.text = commentsJSON.StartWithoutStartingVertexText;
    }
}
