using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class VertexOverviewText {
    public string Vertex;
    public string VisitedVertexes;
    public string UnvisitedVertexes;
    public string NoEdges;
    public string EdgesOrder;
}

[Serializable]
public class CheckingEdgeText {
    public string Vertex;
    public string Edge;
    public string AnotherVertex;
    public string ValidEdge;
    public string InvalidEdge;
}

[Serializable]
public class EndText {
    public string Conclusion;
    public string VisitedVertexes;
    public string UnvisitedVertexes;
}

[Serializable]
public class VertexEndText {
    public string Vertex;
    public string NoUnvisitedVertexes;
    public string ReturnToParent;
}


[Serializable]
public class CommmentsJSON
{
    public string SetUpText;
    public string StartWithoutStartingVertexText;
    public VertexOverviewText VertexOverviewText;
    public CheckingEdgeText CheckingEdgeText;
    public VertexEndText VertexEndText;
    public EndText EndText;
}
