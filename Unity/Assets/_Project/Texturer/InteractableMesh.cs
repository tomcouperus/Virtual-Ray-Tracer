using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class InteractableMesh : MonoBehaviour
{

    public Vertex vertexPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GenerateVertices();
    }

    void GenerateVertices() {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        HashSet<Vector3> vertices = new HashSet<Vector3>(mesh.vertices);
        foreach(Vector3 vertex in vertices) {
            Vertex vert = Instantiate(vertexPrefab, transform);
            vert.SetPosition(vertex);
        }
    }
}
