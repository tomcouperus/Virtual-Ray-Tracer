using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates and displays the wireframe of a mesh.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class InteractableMesh : MonoBehaviour
{

    public GameObject vertexPrefab;
    public Edge edgePrefab;

    GameObject vertices;
    [SerializeField]
    private bool _showVertices;
    /// <summary>
    /// If `true`, shows the vertices of the mesh with the vertexPrefab object.
    /// </summary>
    /// <value></value>
    public bool ShowVertices {
        get {return _showVertices;}
        set {
            _showVertices = value;
            vertices.SetActive(_showVertices);
        }
    }

    GameObject edges;
    [SerializeField]
    private bool _showEdges;
    /// <summary>
    /// If `true`, shows the edges of the mesh with the edgePrefab object.
    /// </summary>
    /// <value></value>
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

    /// <summary>
    /// Generates all vertices of a mesh.
    /// </summary>
    void GenerateVertices() {
        vertices = new GameObject("vertices");
        ShowVertices = _showVertices;
        vertices.transform.parent = transform;
        vertices.transform.localPosition = Vector3.zero;
        vertices.transform.localScale = Vector3.one;
        vertices.transform.Rotate(transform.localEulerAngles, Space.Self);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        HashSet<Vector3> verts = new HashSet<Vector3>(mesh.vertices);
        foreach(Vector3 vertex in verts) {
            GameObject vertexObject = Instantiate(vertexPrefab, vertices.transform);
            vertexObject.transform.localPosition = vertex;
            
            Vector3 scale = vertexObject.transform.localScale;
            scale.x /= transform.localScale.x;
            scale.y /= transform.localScale.y;
            scale.z /= transform.localScale.z;
            vertexObject.transform.localScale = scale;
        }
    }

    /// <summary>
    /// Generates the edges of a mesh.
    /// </summary>
    void GenerateEdges() {
        edges = new GameObject("edges");
        ShowEdges = _showEdges;
        edges.transform.parent = transform;
        edges.transform.localPosition = Vector3.zero;
        edges.transform.Rotate(transform.localEulerAngles, Space.Self);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        int[] triangles = mesh.triangles;

        HashSet<Vector3> createdEdges = new HashSet<Vector3>();
        /** Go triangle by triangle and make edges for each side of the triangle.
          * Removes duplicates based on position, since the edges location is 
          * placed between vertices and thus unique.
          */
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
