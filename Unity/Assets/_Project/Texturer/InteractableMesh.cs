using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class InteractableMesh : MonoBehaviour
{

    public ClickableVertex clickableVertexPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        HashSet<Vector3> vertices = new HashSet<Vector3>(mesh.vertices);
        foreach(Vector3 vertex in vertices) {
            ClickableVertex clickVert = Instantiate(clickableVertexPrefab, transform);
            clickVert.SetPosition(vertex);
        }
    }
}
