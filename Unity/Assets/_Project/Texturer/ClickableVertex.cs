using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableVertex : MonoBehaviour {

    Mesh mesh;
    public int vertexIndex;
    private void OnMouseDown() {
        Debug.Log("Vertex "+vertexIndex+": "+mesh.vertices[vertexIndex]+"; uv: "+mesh.uv[vertexIndex]);
    }

    public void Set(Mesh mesh, int vertexIndex) {
        this.mesh = mesh;
        transform.position = mesh.vertices[vertexIndex] + transform.parent.position;
    }
}
