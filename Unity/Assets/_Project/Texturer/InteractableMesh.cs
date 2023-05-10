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
        for (int i = 0; i < mesh.vertices.Length;i++) {
            ClickableVertex vertex = Instantiate(clickableVertexPrefab, transform);
            vertex.Set(mesh, i);
        }
    }
}
