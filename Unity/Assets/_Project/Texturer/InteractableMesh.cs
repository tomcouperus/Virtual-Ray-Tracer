using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class InteractableMesh : MonoBehaviour
{

    public GameObject vertexPrefab;
    public Edge edgePrefab;

    GameObject vertices;
    private bool _showVertices;
    public bool ShowVertices {
        get {return _showVertices;}
        set {
            _showVertices = value;
            vertices.SetActive(_showVertices);
        }
    }

    GameObject edges;
    private bool _showEdges;
    public bool ShowEdges {
        get {return _showEdges;}
        set {
            _showEdges = value;
            edges.SetActive(_showEdges);
        }
    }

    void Start()
    {
        GenerateVertices();
        GenerateEdges();
    }

    void GenerateVertices() {
        vertices = new GameObject("vertices");
        ShowVertices = false;
        vertices.transform.parent = transform;
        vertices.transform.localPosition = Vector3.zero;
        vertices.transform.Rotate(transform.localEulerAngles, Space.Self);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        HashSet<Vector3> verts = new HashSet<Vector3>(mesh.vertices);
        foreach(Vector3 vertex in verts) {
            GameObject vertexObject = Instantiate(vertexPrefab, vertices.transform);
            vertexObject.transform.localPosition = vertex;
        }
    }

    void GenerateEdges() {
        edges = new GameObject("edges");
        ShowEdges = false;
        edges.transform.parent = transform;
        edges.transform.localPosition = Vector3.zero;
        edges.transform.Rotate(transform.localEulerAngles, Space.Self);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        int[] triangles = mesh.triangles;

        HashSet<Vector3> createdEdges = new HashSet<Vector3>();
        for (int i = 0; i < triangles.Length; i += 3) {
            int vertIndexA = triangles[i];
            int vertIndexB = triangles[i+1];
            int vertIndexC = triangles[i+2];

            Vector3 vertexA = mesh.vertices[vertIndexA];
            Vector3 vertexB = mesh.vertices[vertIndexB];
            Vector3 vertexC = mesh.vertices[vertIndexC];

            Vector3 dirAB = vertexB-vertexA;
            Vector3 posAB = vertexA+(dirAB)/2;
            if (!createdEdges.Contains(posAB)) {
                Edge edgeAB = Instantiate(edgePrefab, edges.transform);
                edgeAB.Direction = dirAB;
                edgeAB.Position = posAB;
                edgeAB.Length = dirAB.magnitude;
                createdEdges.Add(posAB);
            }
            Vector3 dirAC = vertexC-vertexA;
            Vector3 posAC = vertexA+(dirAC)/2;
            if (!createdEdges.Contains(posAC)) {
                Edge edgeAC = Instantiate(edgePrefab, edges.transform);
                edgeAC.Direction = dirAC;
                edgeAC.Position = posAC;
                edgeAC.Length = dirAC.magnitude;
                createdEdges.Add(posAC);
            }
            Vector3 dirBC = vertexC-vertexB;
            Vector3 posBC = vertexB+(dirBC)/2;
            if (!createdEdges.Contains(posBC)) {
                Edge edgeBC = Instantiate(edgePrefab, edges.transform);
                edgeBC.Direction = dirBC;
                edgeBC.Position = posBC;
                edgeBC.Length = dirBC.magnitude;
                createdEdges.Add(posBC);
            }

        }
    }
}
