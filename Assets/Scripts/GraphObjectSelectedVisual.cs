using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphObjectSelectedVisual : MonoBehaviour
{
    [SerializeField] private GraphObject graphObject;
    [SerializeField] private GameObject visualGameObject;

    private void Start() {
        Graph.Instance.OnSelectedGraphObjectChanged += Graph_OnSelectedGraphObjectChanged;

        Hide();
    }

    private void Graph_OnSelectedGraphObjectChanged(object sender, Graph.OnSelectedGraphObjectChangedEventargs e) {
        if (graphObject != null) {
            if (e.selectedGraphObject != graphObject) {
                Hide();
            } else {
                Show();
            }
        }
    }

    private void Show() {
        visualGameObject.SetActive(true);
    }

    private void Hide() {
        visualGameObject.SetActive(false);
    }
}
