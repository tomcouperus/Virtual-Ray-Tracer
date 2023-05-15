using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMapper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        
        Mesh newMesh = new Mesh();
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < mesh.vertexCount; i++) {
            Vector2 uv = mesh.uv[i];
            vertices[i] = new Vector3(uv.x, 0, uv.y);
        }
        newMesh.vertices = vertices;
        newMesh.triangles = mesh.triangles;
        newMesh.uv = mesh.uv;
        newMesh.RecalculateNormals();
        meshFilter.mesh = newMesh;
    }
}
