using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class InteractableMesh : MonoBehaviour
{
    public float verticesScale = 0.03f;
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length;i++) {
            Vector3 vPos = vertices[i];
            GameObject vertex = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            vertex.name = "vertex "+i;
            vertex.transform.SetParent(transform);
            vertex.transform.position = vPos;
            vertex.transform.localScale = Vector3.one * verticesScale;

            ClickableVertex clickableVertex = vertex.AddComponent<ClickableVertex>();
            clickableVertex.SetVertex(i, vPos, mesh.uv[i]);
        }
    }
}
